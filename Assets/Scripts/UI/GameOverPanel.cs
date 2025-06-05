using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_victoryBG;
    [SerializeField] private GameObject m_defeatBG;
    [SerializeField] private TMP_Text m_honorText;

    private void Start()
    {
        GameProgress.Instance.OnDataChanged += OnHonorChanged;
    }

    private void OnDestroy()
    {
        GameProgress.Instance.OnDataChanged -= OnHonorChanged;
    }

    public void OnHonorChanged()
    {
        int honor = GameProgress.Instance.Data.Honor;
        m_honorText.text = "Honor: " + honor;

        Debug.Log("honor changed : " + honor);
    }

    public void DisplayEndGame(bool victory)
    {
        GameProgress.Instance.OnDataChanged += OnHonorChanged;
        if (victory)
        {
            m_defeatBG.SetActive(false);
            m_victoryBG.SetActive(true);
        }
        else
        {
            m_defeatBG.SetActive(true);
            m_victoryBG.SetActive(false);
        }
    }
    
    public void OnSkipRewardButtonPressed()
    {
        SceneController.Instance.LoadScene(Scenes.Map);
    }
}
