using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TestDeckTemplateSelect m_deckTemplateSelect;

    private void Start()
    {
        m_deckTemplateSelect.Init();
    }

    public void PlayClicked()
    {
        SceneController.Instance.LoadScene(Scenes.Game);
    }

    public void MapClicked()
    {
        SceneController.Instance.LoadScene(Scenes.Map);
    }

    public void DeleteDataClicked()
    {
        string path = Application.persistentDataPath;

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true); 
        }
    }
}