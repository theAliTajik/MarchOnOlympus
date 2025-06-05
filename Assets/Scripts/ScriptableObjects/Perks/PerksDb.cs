using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static EnemiesDb;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "PerksDb", menuName = "Olympus/Perks Db")]
public class PerksDb : GenericData<PerksDb>
{
    [System.Serializable]
    public class PerksInfo
    {
        [FormerlySerializedAs("clientID")] public string ClientID;
        [FormerlySerializedAs("icon")] public Sprite Icon;
        [FormerlySerializedAs("desc")] [Multiline]
        public string Desc;

        public bool Invisible;
        [FormerlySerializedAs("perkData")] public BasePerkData PerkData;

        [FormerlySerializedAs("Implemented")] public bool IsImplemented;
        public string ActionScriptName;

        public string GetScriptName()
        {
            if (!string.IsNullOrEmpty(ActionScriptName))
            {
                return ActionScriptName + "Perk";
            }
            
            return ClientID + "Perk";
        }
    }

    public List<PerksInfo> AllPerks;
    
    [SerializeField] private Sprite defaultIcon;


#if UNITY_EDITOR
    private string spritesFolderPath = "Assets/Sprites/Perks";
    
    [ContextMenu("Assign Sprites")]
    void assignSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { spritesFolderPath });

        Sprite[] allSprites = guids.Select(guid => AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
        
        foreach (var perk in AllPerks)
        {
            string clientIDPrefix = perk.ClientID;
            Sprite matchingSprite = allSprites.FirstOrDefault(sprite => sprite.name.StartsWith(clientIDPrefix));

            if (matchingSprite != null)
            {
                perk.Icon = matchingSprite;
                Debug.Log($"Assigned {matchingSprite.name} to {perk.ClientID}");
            }
            else
            {
                Debug.LogWarning($"No matching sprite found for {perk.ClientID}");
            }
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

    public PerksInfo FindById(string clientId)
    {
        for (int i = 0; i < AllPerks.Count; i++)
        {
            PerksInfo perk = AllPerks[i];
            if (perk.ClientID == clientId)
            {
                return perk;
            }
        }

        return null;
    }

    public PerksInfo CreatePerk(string clientID, Sprite icon, string desc, BasePerkData perkData)
    {
        PerksInfo perk = new PerksInfo();
        perk.ClientID = clientID;
        if (icon != null)
        {
            perk.Icon = icon;
        }
        else
        {
            perk.Icon = defaultIcon;
        }

        if (desc != null && desc != string.Empty)
        {
            perk.Desc = desc;
        }
        else
        {
            perk.Desc = "Temprary Description for Perk " + clientID;
        }

        if (perkData != null)
        {
            perk.PerkData = perkData;
        }
        perk.IsImplemented = true;
        AllPerks.Add(perk);
        return perk;
    }

    public void CreatePerk(string clientID, Sprite icon, string desc, BasePerkData perkData, string actionScriptName)
    {
        PerksInfo perk = CreatePerk(clientID, icon, desc, perkData);
        perk.ActionScriptName = actionScriptName;
    }

    public PerksInfo GetRandPerk()
    {
        PerksInfo randPerk;
        do
        {
            randPerk = AllPerks[Random.Range(0, AllPerks.Count)];
        } while (randPerk.PerkData?.Rarity != PerkRarity.NORMAL);
        return randPerk;
    }

    public PerksInfo GetRandLegenPerk()
    {
        List<PerksInfo> allLegenPerks = new List<PerksInfo>();
        for (int i = 0; i < AllPerks.Count; i++)
        {
            if (AllPerks[i].PerkData == null)
            {
                continue;
            }
            
            if (AllPerks[i].PerkData.Rarity == PerkRarity.LEGENDARY)
            {
                allLegenPerks.Add(AllPerks[i]);
            }
        }

        if (allLegenPerks.Count == 0)
        {
            return null;
        }
        
        PerksInfo randPerk;
        randPerk = allLegenPerks[Random.Range(0, allLegenPerks.Count)];
        
        return randPerk;
    }
}