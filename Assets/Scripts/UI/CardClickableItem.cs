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

public class CardClickableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public event Action<CardDisplay> OnPointerEntered;
	public event Action<CardDisplay> OnPointerExited;
	public event Action<CardClickableItem> OnClick;



	[SerializeField] private GameObject m_StanceIcon;
	[SerializeField] private GameObject m_StanceIconGlow;
	[SerializeField] private TMP_Text m_nameText;
	[SerializeField] private TMP_Text m_descriptionText;
	[SerializeField] private TMP_Text m_stanceDescriptionText;
    [SerializeField] private TMP_Text m_cardClassText;
    [SerializeField] private GameObject m_StanceDescriptionIcon;
	[SerializeField] private TMP_Text m_manaText;
	[SerializeField] private Image m_artworkImage;

	private RectTransform m_rectTransform;
	
	private CardInDeckStateMachine m_cardInDeck;
	
	private CardDisplay m_cardDisplay;

	public CardDisplay CardDisplay => m_cardDisplay;
	public CardInDeckStateMachine CardInDeck => m_cardInDeck;
	
	public void Configure(CardDisplay cardDisplay)
	{
		m_cardDisplay = cardDisplay;
		m_cardInDeck = cardDisplay.CardInDeck;
		m_cardInDeck.OnDataChanged += RefreshUI;
	}
	
	public void Configure(BaseCardData cardData)
	{
		CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
		cardInDeck.Configure(cardData);
		m_cardInDeck = cardInDeck;
		m_cardInDeck.OnDataChanged += RefreshUI;
	}

	public void RefreshUI()
	{
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
		
		m_StanceIconGlow.SetActive(false);
		
		
		m_manaText.color = Color.white;
		
		Sprite image = m_cardInDeck.GetCardImage();
		if(image != null)
			m_artworkImage.sprite = image;

		m_manaText.text = m_cardInDeck.CurrentState.GetEnergy().ToString();
	}

	
	public void OnPointerEnter(PointerEventData eventData)
	{
	}
	public void OnPointerExit(PointerEventData eventData)
	{
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke(this);
	}
}
