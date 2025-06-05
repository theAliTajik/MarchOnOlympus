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



[CreateAssetMenu(fileName = "CardsDb", menuName = "Olympus/Cards Db")]
public class CardsDb : GenericData<CardsDb>
{
    [System.Serializable]
    public class CardsInfo
    {
        public string clientID;
        public BaseCardData CardData;

        public bool IsImplemented;

    }

    public List<CardsInfo> AllCards;
    


#if UNITY_EDITOR
    private string CardsFolderPath = "Assets/Data/Resources/CardsData";
    
    [ContextMenu("Make cards")]
    void MakeAllCardsFromFolder()
    {
        List<BaseCardData> cards = LoadAllScriptableObjects<BaseCardData>(CardsFolderPath);
        
        foreach (BaseCardData card in cards)
        {
            CardsInfo cardInfo = new CardsInfo();
            cardInfo.clientID = card.name;
            cardInfo.CardData = card;
            cardInfo.IsImplemented = true;
            
            AllCards.Add(cardInfo);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
    
    
    public static List<BaseCardData> LoadAllScriptableObjects<BaseCardData>(string folderPath) where BaseCardData : ScriptableObject
    {
        List<BaseCardData> scriptableObjects = new List<BaseCardData>();

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(BaseCardData).Name, new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            BaseCardData asset = AssetDatabase.LoadAssetAtPath<BaseCardData>(assetPath);

            if (asset != null)
            {
                scriptableObjects.Add(asset);
            }
        }

        return scriptableObjects;
    }
#endif

    public CardsInfo FindById(string clientId)
    {
        clientId = clientId.ToLower();
        for (int i = 0; i < AllCards.Count; i++)
        {
            CardsInfo card = AllCards[i];
            if (card.clientID.ToLower() == clientId)
            {
                return card;
            }
        }

        return null;
    }

    public void CreateCard(BaseCardData cardData)
    {
        CardsInfo card = new CardsInfo();
        card.clientID = cardData.name;
        card.CardData = cardData;
        
        card.IsImplemented = true;
        
        AllCards.Add(card);
    }

    public BaseCardData GetRandom()
    {
        return AllCards[UnityEngine.Random.Range(0, AllCards.Count)].CardData;
    }

    public BaseCardData GetRandomLegen()
    {
        List<BaseCardData> legenCards = new List<BaseCardData>();
        for (var i = 0; i < AllCards.Count; i++)
        {
            if (AllCards[i].CardData.Rarity == CardRarity.LEGENDARY)
            {
                legenCards.Add(AllCards[i].CardData);
            }
        }
        
        return legenCards[UnityEngine.Random.Range(0, legenCards.Count)];
    }
    
    public BaseCardData GetRandomFromPacks(List<CardPacks> packs)
    {
        List<BaseCardData> cardsInPack = new List<BaseCardData>();
        for (int i = 0; i < AllCards.Count; i++)
        {
            foreach (CardPacks pack in packs)
            {
                if (pack == AllCards[i].CardData.CardPack)
                {
                    cardsInPack.Add(AllCards[i].CardData);
                    break;
                }
            }
        }

        return cardsInPack[UnityEngine.Random.Range(0, cardsInPack.Count)];
    }
}