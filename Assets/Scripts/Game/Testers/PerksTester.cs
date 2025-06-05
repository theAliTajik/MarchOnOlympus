#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PerksTester : MonoBehaviour
{
    public PerksManager PerksManager;
    
    public List<BasePerk> perks;


    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("w");
            foreach (BasePerk perk in perks)
            {
                PerksManager.AddPerk(perk);
            }
        }        
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("b");
            foreach (BasePerk perk in perks)
            {
                PerksManager.RemovePerk(perk);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PerksManager.OnPhaseChange(EGamePhase.PLAYER_TURN_END);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerksManager.AddPerk("Blessed");
        }
        */
    }

    public void AddButtonClick()
    {

    }
}

#endif