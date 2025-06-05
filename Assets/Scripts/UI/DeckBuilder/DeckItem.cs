using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeckItem : MonoBehaviour
{
    public Action<DeckItem> OnClicked;
    public Action<DeckItem> OnAddClicked;
    public Action<DeckItem> OnRemoveClicked;
    public Action<DeckItem> OnDuplicateClicked;
    public Action<DeckItem> OnDuplicateDeckClicked;


    [SerializeField] private TMP_Text m_nameText;

    [SerializeField] private Button m_upButton;

    [SerializeField] private Button m_addButton;
    [SerializeField] private Button m_removeButton;
    [SerializeField] private Button m_duplicateButton;
    [SerializeField] private Button m_duplicateDeckButton;


    [SerializeField] private Button m_ClickButton;



    private string m_id;
    private int m_index = -1;
    private bool m_isAdd;

    public string ID { get { return m_id; } }
    public int Index { get { return m_index; } }


    private void Awake()
    {
        SubscribeToAllButtons();
        HideAllButtons();
    }

    private void SubscribeToAllButtons()
    {
        m_ClickButton.onClick.AddListener(OnClick);

        m_upButton.onClick.AddListener(OnAddButtonClicked);
        m_addButton.onClick.AddListener(OnAddButtonClicked);
        m_removeButton.onClick.AddListener(OnRemoveButtonClicked);
        m_duplicateButton.onClick.AddListener(OnDuplicateButtonClicked);
        m_duplicateDeckButton.onClick.AddListener(OnDuplicateButtonClicked);
    }

    private void HideAllButtons()
    {
        m_upButton.gameObject.SetActive(false);
        m_addButton.gameObject.SetActive(false);
        m_removeButton.gameObject.SetActive(false);
        m_duplicateButton.gameObject.SetActive(false);
        m_duplicateDeckButton.gameObject.SetActive(false);
    }

    public DeckItem Reset()
    {
        HideAllButtons();
        m_index = -1;
        return this;
    }


    public void OnClick()
    {
        if (m_isAdd)
        {
            OnAddButtonClicked();
        }
        else
        {
            OnClicked?.Invoke(this);
        }
    }

    public void OnAddButtonClicked()
    {
        OnAddClicked?.Invoke(this);

    }

    public void OnRemoveButtonClicked()
    {
        OnRemoveClicked?.Invoke(this);
    }
    
    public void OnDuplicateButtonClicked()
    {
        OnDuplicateClicked?.Invoke(this);
    }

    public void OnDuplicateDeckButtonClicked()
    {
        OnDuplicateDeckClicked?.Invoke(this);
    }

    // config with extensions

    public DeckItem EnableAdd()
    {
        m_isAdd = true;
        m_addButton.gameObject.SetActive(true);
        return this;
    }
    
    public DeckItem EnableRemove()
    {
        m_isAdd = false;
        m_removeButton.gameObject.SetActive(true);
        return this;
    }

    public DeckItem EnableDuplicate()
    {
        m_isAdd = false;
        m_duplicateDeckButton.gameObject.SetActive(true);
        return this;
    }
    
    public DeckItem EnableDuplicateDeck()
    {
        m_isAdd = false;
        m_duplicateDeckButton.gameObject.SetActive(true);
        return this;
    }

    public DeckItem SetID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.Log("Deck item id was set but null or empty");
            return this;
        }
        m_id = id;
        m_nameText.text = id;
        return this;
    }

    public DeckItem SetIndex(int index)
    {
        m_index = index;
        return this;
    }

}

