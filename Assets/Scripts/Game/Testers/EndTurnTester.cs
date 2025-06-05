using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnTester : MonoBehaviour
{
    public EndTurnWidget m_EndTurnWidget;
    public Button m_SetEnemyTurnButton;
    public Button m_SetPlayerTurnNormalButton;
    public Button m_SetPlayerTurnActiveButton;

    /*
    void Start()
    {
        m_SetEnemyTurnButton.onClick.AddListener(OnSetEnemyTurnButtonPressed);
        m_SetPlayerTurnNormalButton.onClick.AddListener(OnSetPlayerTurnNormalButtonPressed);
        m_SetPlayerTurnActiveButton.onClick.AddListener(OnSetPlayerTurnActiveButtonPressed);
    }

    void OnSetEnemyTurnButtonPressed()
    {
        m_EndTurnWidget.SetEnemyTurn();
    }

    void OnSetPlayerTurnNormalButtonPressed()
    {
        m_EndTurnWidget.SetPlayerTurn(false);
    }

    void OnSetPlayerTurnActiveButtonPressed()
    {
        m_EndTurnWidget.SetPlayerTurn(true);
    }
    */
}
