using System;
using System.Collections;
using System.Collections.Generic;
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
}