using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanGeneralCaptain1 : TrojanGeneralCaptain
{
    [SerializeField] private Captain1MovesData m_data;

    private void Awake()
    {
        base.Awake();
        m_actionData = m_data.Move1Str;
        m_hp = m_data.HP;
        m_numOfTurnsChanneledRemaining = m_data.NumOfTurnsToChannel;
        ConfigFighterHP();
    }
    
    public override void DetermineIntention()
    {
        if (m_numOfTurnsChanneledRemaining <= 0)
        {
            m_numOfTurnsChanneledRemaining = m_data.NumOfTurnsToChannel;
            m_nextMove = m_specialmovesDatas[0];
        }
        else
        {
            RandomIntentionPicker(m_moves);
        }
        ShowIntention();
    }
}
