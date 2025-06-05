using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum CardState
{
	IDLE,
	HOVER,
	DRAGING,
	SELECTED,
	DEACTIVE,
}
public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler,
	IEndDragHandler, IPointerClickHandler
{
	public event Action<CardDisplay> OnDragEnd;
	public event Action<CardDisplay> OnPointerEntered;
	public event Action<CardDisplay> OnPointerExited;
	public event Action<CardDisplay> OnPlay;
	public event Action<CardDisplay> OnDeselect;
	public event Action<CardDisplay> OnClick;



	[SerializeField] private GameObject m_StanceIcon;
	[SerializeField] private GameObject m_StanceIconGlow;
	[SerializeField] private TMP_Text m_nameText;
	[SerializeField] private TMP_Text m_descriptionText;
	[SerializeField] private TMP_Text m_stanceDescriptionText;
    [SerializeField] private TMP_Text m_cardClassText;
    [SerializeField] private GameObject m_StanceDescriptionIcon;
	[SerializeField] private TMP_Text m_manaText;
	[SerializeField] private Image m_artworkImage;

	[SerializeField] private float m_selectY;

	private RectTransform m_rectTransform;
	private CardState m_cardState = CardState.IDLE;
	private bool m_manaIsEnough;
	private CardInDeckStateMachine m_cardInDeck;
	private float m_canvasScaleFactor;
	private Stance m_stance = Stance.NONE;
    private bool m_isMoving;
    
    public bool ManaIsEnough => m_manaIsEnough;
    public CardState CardState => m_cardState;
    public CardInDeckStateMachine CardInDeck
    {
	    get => m_cardInDeck;
	    set => m_cardInDeck = value;
    }

    public RectTransform RectTransform => m_rectTransform;

    public float CanvasScaleFactor => m_canvasScaleFactor;
    public float SelectY => m_selectY;
    
    private void Awake()
	{



	}

	void Start()
	{

		
	}
	
	private void OnDestroy()
	{
		if (m_cardInDeck != null) m_cardInDeck.OnDataChanged -= RefreshUI;
		GameplayEvents.OnEnergyChanged -= OnEnergyChange;
	}


	private void OnEnergyChange(int energy)
	{
		if (m_cardInDeck.CurrentState.GetEnergy() > energy)
		{
			SetEnoughMana(false);
		}
		else
		{
			SetEnoughMana(true);
		}
	}

	public void Configure(BaseCardData cardData)
	{
        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(cardData);

        // Debug.Log(cardData.GetInstanceID());
        Configure(cardInDeck);
	}
	
	
	public void Configure(CardInDeckStateMachine cardInDeck)
	{
		m_cardInDeck = cardInDeck;
		m_cardInDeck.OnDataChanged += RefreshUI;
		
		m_rectTransform = GetComponent<RectTransform>();
		GameplayEvents.OnEnergyChanged += OnEnergyChange;

		if (CombatManager.Instance)
		{
			SetStance(CombatManager.Instance.CurrentStance);
		}
		// Debug.Log(cardInDeck.GetCardData().GetInstanceID());
		RefreshUI();
	}
	
	public void Configure(CardInDeckStateMachine card, float canvasScale)
	{
		m_cardInDeck = card;
		m_canvasScaleFactor = canvasScale;
		m_cardInDeck.OnDataChanged += RefreshUI;
		
		m_rectTransform = GetComponent<RectTransform>();
		GameplayEvents.OnEnergyChanged += OnEnergyChange;
		
		SetStance(CombatManager.Instance.CurrentStance);
		// Debug.Log(card.GetCardData().GetInstanceID());
		RefreshUI();
	}

	public void RefreshUI()
	{
		if (CombatManager.Instance)
		{
			m_stance = CombatManager.Instance.CurrentStance;
			m_cardInDeck.ChangeState(m_stance);
		}
		
		m_nameText.text = m_cardInDeck.GetCardName();
		m_descriptionText.text = m_cardInDeck.NormalState.GetDescription();
		m_stanceDescriptionText.text = m_cardInDeck.StanceState.GetDescription();
		m_cardClassText.text = m_cardInDeck.GetCardPack().ToString();
		
		if (m_stanceDescriptionText.text == "")
		{
			m_StanceIcon.SetActive(false);
			m_StanceIconGlow.SetActive(false);
			m_StanceDescriptionIcon.SetActive(false);
		}
		else
		{
			m_StanceIcon.SetActive(true);
			m_stanceDescriptionText.gameObject.SetActive(true);
			m_StanceDescriptionIcon.SetActive(true);
		}
		
		if (m_stance != Stance.NONE && m_stance == m_cardInDeck.GetStance())
		{
			m_StanceIconGlow.SetActive(true);
		}
		else
		{
			m_StanceIconGlow.SetActive(false);
		}
		
		if (m_manaIsEnough)
		{
			m_manaText.color = Color.white;
		}
		else
		{
			m_manaText.color = Color.red;
		}
		
		Sprite image = m_cardInDeck.GetCardImage();
		if(image != null)
			m_artworkImage.sprite = image;

		m_manaText.text = m_cardInDeck.CurrentState.GetEnergy().ToString();
	}

	bool hasEnergyBeenChanged = false;
	private int m_originalEnergyCost;
	private int m_stanceOriginalEnergyCost;
	
	
	public void SetStance(Stance stance)
	{
		RefreshUI();
	}

	public void SetState(CardState newState)
	{
		m_cardState = newState;
		if (newState == CardState.IDLE)
		{
			RefreshUI();
		}
	}

	public void SetEnoughMana(bool isEnoughMana)
	{
		m_manaIsEnough = isEnoughMana;
		RefreshUI();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (m_isMoving)
		{
			return;
		}

		if (m_cardState == CardState.IDLE)
		{
			m_cardState = CardState.HOVER;
			OnPointerEntered?.Invoke(this);
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
        if (m_isMoving)
        {
            return;
        }
        
        if (m_cardState == CardState.HOVER)
		{
			m_cardState = CardState.IDLE;
			OnPointerExited?.Invoke(this);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
        if (m_isMoving)
        {
            return;
        }

        if (m_cardState != CardState.DEACTIVE && m_manaIsEnough)
		{
			m_cardState = CardState.DRAGING;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
        if (m_isMoving)
        {
            return;
        }

        if ((m_cardState == CardState.SELECTED && m_cardInDeck.CurrentState.GetTargetingType() == TargetType.ENEMY) || m_cardState == CardState.DEACTIVE)
		{
			return;
		}

		switch (m_cardInDeck.CurrentState.GetTargetingType())
		{
			case TargetType.PLAYER:
			case TargetType.PLAYER_ENEMY:
				RectTransform.anchoredPosition += eventData.delta / m_canvasScaleFactor;
				if (RectTransform.anchoredPosition.y >= m_selectY && m_cardState != CardState.SELECTED)
				{
					if (m_manaIsEnough)
					{
						m_cardState = CardState.SELECTED;
						OnPlay?.Invoke(this);
					}
				} else if (m_cardState == CardState.SELECTED && RectTransform.anchoredPosition.y <= m_selectY)
				{
					OnDeselect?.Invoke(this);
				}
				break;
			case TargetType.ENEMY:
				if (RectTransform.anchoredPosition.y >= m_selectY)
				{
					if (m_manaIsEnough)
					{
						m_cardState = CardState.SELECTED;
						OnPlay?.Invoke(this);
						RectTransform.DOAnchorPos(new Vector2(30, 250), 0.1f);
						return;
					}
					OnPlay?.Invoke(this);
				}
				RectTransform.anchoredPosition += eventData.delta / m_canvasScaleFactor;
				break;
		}
	}
	public void OnEndDrag(PointerEventData eventData)
	{
        if (m_isMoving)
        {
            return;
        }

        if (m_cardState != CardState.SELECTED)
		{
			OnDragEnd?.Invoke(this);
		}
	}

    public void MoveTo(Vector2 v, float duration)
    {
	    SetState(CardState.IDLE);
		m_isMoving = true;
        m_rectTransform.DOAnchorPos(v, duration).SetUpdate(true).OnComplete(()=> m_isMoving = false);
		m_rectTransform.DOLocalRotate(new Vector3(0, 0, 0), duration).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
	    OnClick?.Invoke(this);
    }
}
