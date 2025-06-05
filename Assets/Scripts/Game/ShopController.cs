using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_honorText;
    private const int m_honorGainAmount = 5;


    private void Start()
    {
        MapNodeSaveManager.MarkNodeCompleted();
        GameProgress.Instance.OnDataChanged += OnHonorChanged;
        OnHonorChanged();
    }

    private void OnDestroy()
    {
        if (GameProgress.Instance)
        {
            GameProgress.Instance.OnDataChanged -= OnHonorChanged;
        }
    }

    public void OnHonorChanged()
    {
        int honor = GameProgress.Instance.Data.Honor;
        m_honorText.text = "Honor: " + honor;

        Debug.Log("honor changed : " + honor);
    }

    public void OnAddHonorClicked()
    {
        GameProgress.Instance.Data.Honor += m_honorGainAmount;
    }
    
    public void OnBackToMapClicked()
    {
        SceneController.Instance.LoadScene(Scenes.Map);
    }
}
