using System;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;
using UnityEngine.UI;

public class InventTester : MonoBehaviour
{
    public InventModel Model;
    public List<InventFinisher> Invents;
    
    private void Awake()
    {
        // Model.SetFinishers(Invents);
        SaveTestFinisherToJson();
    }

    private void SaveTestFinisherToJson()
    {
        List<string> finishers =  new List<string>()
        {
            "TrojanHorse"
        };
        GameProgress.Instance.Data.InventFinisherIds = finishers;
    }

    [Command("Gain_invent")]
    public void AddInvent(int amount)
    {
        GameActionHelper.GainInvent(amount);
    }
}