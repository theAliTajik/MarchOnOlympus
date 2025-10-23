
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventsDb", menuName = "Olympus/InventsDb")]
public class InventsDb : GenericData<InventsDb>
{
    [SerializeField] private List<InventFinisher> InventFinishers;


    public InventFinisher FindById(string ID)
    {
        var finisher = InventFinishers.Find(x => x.ID == ID);

        if (finisher == null)
        {
            CustomDebug.LogWarning($"Did not find invent finisher with ID: {ID}", Categories.Combat.Invent.Root);
        }
        
        return finisher;
    }
}
