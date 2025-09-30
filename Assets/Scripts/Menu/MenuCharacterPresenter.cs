using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuCharacterPresenter : MonoBehaviour
{
    [SerializeField] private Image m_charSelectorMenuHeroImage;
    [SerializeField] private Image m_charSelectorMenuNameImage;

    [SerializeField] private Image m_mainMenuCharMiddleImage;
    [SerializeField] private TMP_Text m_mainMenuCharName;
    
    [SerializeField] private ISelector<string> m_verticalSelector;
    [SerializeField] private ISelector<string> m_incrementalSelector;
    
    private const string CHAR_SELECTED_PLAYER_PREFS = "SelectedPlayer";
    
    private SelectionData m_selectionData;

    private void Awake()
    {

        m_verticalSelector.OnSelect += OnCharSelected;
        m_incrementalSelector.OnSelect += OnCharSelected;

        var DisplayDataEnumerator = ConvertCharInfoToDisplayData(CharactersDb.Instance.GetEnumerator());
        m_selectionData = new SelectionData(DisplayDataEnumerator, efficientMode: true);
        m_selectionData.CurrentlySelected = FindCurrentCharacter(DisplayDataEnumerator);
        OnCharSelected(m_selectionData.CurrentlySelected.Id);

        
        if (m_verticalSelector == null || m_incrementalSelector == null)
        {
            CustomDebug.LogError("WARNING: Missing selector", Categories.UI.MainMenu);
            return;
        }
        
        m_verticalSelector.StartSelect(m_selectionData);
        m_incrementalSelector.StartSelect(m_selectionData);
    }

    private SelectableItemDisplayData FindCurrentCharacter(IEnumerable<SelectableItemDisplayData> displaym_selectionData)
    {
        var firstOrDefault = displaym_selectionData.FirstOrDefault();
        if (!PlayerPrefs.HasKey(CHAR_SELECTED_PLAYER_PREFS))
        {
            return firstOrDefault;
        }
        
        string id = PlayerPrefs.GetString(CHAR_SELECTED_PLAYER_PREFS);
        if (string.IsNullOrEmpty(id))
        {
            return firstOrDefault;
        }
        
        var prefsSelection = displaym_selectionData.FirstOrDefault(x => x.Id == id);
        
        return prefsSelection;
    }

    private IEnumerable<SelectableItemDisplayData> ConvertCharInfoToDisplayData(IEnumerator<CharacterInfo> characters)
    {
        List<SelectableItemDisplayData> displayDataList = new List<SelectableItemDisplayData>();
        while (characters.MoveNext())
        {
            CharacterInfo current = characters.Current;
            SelectableItemDisplayData convertedCurrent = ConvertCharInfoToDisplayData(current);
            displayDataList.Add(convertedCurrent);
        }

        return displayDataList;
    }

    private SelectableItemDisplayData ConvertCharInfoToDisplayData(CharacterInfo characters)
    {
        var displayData = new SelectableItemDisplayData
        {
            Id = characters.Id,
            Text = characters.Name,
            Sprite = characters.Icon
        };
        
        return displayData;
    }

    private void OnCharSelected(string id)
    {
        CharacterInfo info = CharactersDb.Instance.FindById(id);
        if (string.IsNullOrEmpty(info.Id))
        {
            Debug.Log("ERROR: character info for item clicked not found");
            return;
        }

        if (m_charSelectorMenuHeroImage != null)
        {
            m_charSelectorMenuHeroImage.sprite = info.HeroImage;
            m_mainMenuCharMiddleImage.sprite = info.HeroImage;
        }

        if (m_charSelectorMenuNameImage != null)
        {
            m_charSelectorMenuNameImage.sprite = info.NameImage;
            m_mainMenuCharName.text = info.Name;
        }

        m_selectionData.CurrentlySelected = ConvertCharInfoToDisplayData(info);
        m_incrementalSelector.UpdateData(m_selectionData);
        GameSessionParams.CharacterId = info.Id;
        PlayerPrefs.SetString(CHAR_SELECTED_PLAYER_PREFS, info.Id);
    }
}
