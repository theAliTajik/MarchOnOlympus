using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using static EnemiesDb;


[System.Serializable]
public class CardsInfo
{
    public string clientID;
    public BaseCardData CardData;

    public bool IsImplemented;
    public bool IsUpgrade;
    public bool IsOdyCard;
}


[CreateAssetMenu(fileName = "CardsDb", menuName = "Olympus/Cards Db")]
public class CardsDb : GenericData<CardsDb>
{
    public List<CardsInfo> AllLoadedCards;

    [SerializeField] private List<CardInfoContainer> AllCardContainers;
    


#if UNITY_EDITOR
    private string CardsFolderPath = "Assets/Data/Resources/CardsData";

    [ContextMenu("Separate upgrade cards")]
    void SeparateUpgradeOdyCards()
    {
        List<CardsInfo> upgradeCards = new List<CardsInfo>();
        foreach (var card in AllLoadedCards)
        {
            if(card.clientID.EndsWith("_STAR") || card.clientID.EndsWith("_PLUS"))
            {
                upgradeCards.Add(card);
            }
        }
        
        var upgradeContainer = new CardInfoContainer();
        upgradeContainer.AllCards = upgradeCards;
        AllCardContainers.Add(upgradeContainer);
    }

    [ContextMenu("separate ody cards")]
    void MoveOdyCardsFromAllToContainer()
    {
        var odyCardNames = GetOdyCardNames();

        List<CardsInfo> MatchedCards = new List<CardsInfo>();
        foreach (var odyCard in odyCardNames)
        {
            var match = AllLoadedCards.Find(c => c.clientID.ToLower() == odyCard);

            if (match != null)
            {
                MatchedCards.Add(match);
            }
            else
            {
                Debug.Log("Did not find card: " + odyCard);
            }
        }
        
        var container = new CardInfoContainer();
        container.AllCards = MatchedCards;
        AllCardContainers.Add(container);
    }

    [ContextMenu("separate Akh cards")]
    void SeparateAkhCards()
    {
        List<CardsInfo> AkhCards = new();
        foreach (var card in AllLoadedCards)
        {
            var cardMatch = AllCardContainers[0].AllCards.Find(x => x.clientID == card.clientID);
            if (cardMatch != null)
            {
                continue;
            }
            
            cardMatch = AllCardContainers[1].AllCards.Find(x => x.clientID == card.clientID);
            if (cardMatch != null)
            {
                continue;
            }
            
            AkhCards.Add(card);
        }
        
        var container = new CardInfoContainer();
        container.AllCards = AkhCards;
        AllCardContainers.Add(container);
    }


    List<string> OdyCardNamesReadFromFile()
    {
        List<string> odyCardNames = new List<string>(File.ReadAllLines("Assets/Scripts/ScriptableObjects/Cards/OdyCardsName.csv"));
        return odyCardNames;
    }

    List<string> ParseOdyCardNames(List<string> odyNames)
    {
        List<string> odyCardNames = new List<string>();
        foreach (var name in odyNames)
        {
            odyCardNames.Add(name.ToLower().Replace(" ", ""));
        }
        return odyCardNames;
    }

    List<string> GetOdyCardNames()
    {
        var fileName = OdyCardNamesReadFromFile();
        var parsedNames = ParseOdyCardNames(fileName);

        return parsedNames;
    }
    
    [ContextMenu("Make cards")]
    void MakeAllCardsFromFolder()
    {
        // CustomDebug.LogError("This method is not maintained and does not work", Categories.Data.Cards);
        List<BaseCardData> cards = LoadAllScriptableObjects<BaseCardData>(CardsFolderPath);
        
        foreach (BaseCardData card in cards)
        {
            CardsInfo cardInfo = new CardsInfo();
            cardInfo.clientID = card.name;
            cardInfo.CardData = card;
            cardInfo.IsImplemented = true;
            
            AllLoadedCards.Add(cardInfo);
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
        for(int i = 0; i < AllCardContainers.Count; i++)
        {
            CardsInfo card = AllCardContainers[i].FindById(clientId);
            if (card != null)
            {
                return card;
            }
        }

        return null;
    }

    public string GetID(BaseCardData cardData)
    {
        for(int i = 0; i < AllCardContainers.Count; i++)
        {
            string id = AllCardContainers[i].GetID(cardData);
            if (!string.IsNullOrEmpty(id))
            {
                return id;
            }
        }
        
        return null;
    }

    public void CreateCard(BaseCardData cardData)
    {
        AllCardContainers[0].CreateCard(cardData);
    }

    public BaseCardData GetRandom()
    {
        string id = GameSessionParams.CharacterId;
        var container = GetCardContainerForCharacter(id);
        return container.GetRandom();
    }

    private CardInfoContainer GetCardContainerForCharacter(string charId)
    {
        var container = AllCardContainers.Find(CC => CC.GetCharId() == charId);
        if (container == null)
        {
            CustomDebug.LogError("Did not find character with id: " + charId, Categories.Data.Cards);
        }
        
        return container;
    }

    public BaseCardData GetRandomLegen()
    {
        string id = GameSessionParams.CharacterId;
        var container = GetCardContainerForCharacter(id);
        return container.GetRandomLegen();
    }
    
    public BaseCardData GetRandomFromPacks(List<CardPacks> packs)
    {
        string id = GameSessionParams.CharacterId;
        var container = GetCardContainerForCharacter(id);
        return container.GetRandomFromPacks(packs);
    }
    
    public List<BaseCardData> GetCardsWithName(string partialName, bool contains)
    {
        foreach (var container in AllCardContainers)
        {
            var card = container.GetCardsWithName(partialName, contains);

            if (card != null)
            {
                return card;
            }
        }
        
        return null;
    }

    public List<BaseCardData> GetAllImplementedCards()
    {
        List<BaseCardData> allCard = new();
        foreach (var container in AllCardContainers)
        {
            allCard.AddRange(container.GetAllImplementedCards());
        }
        
        return allCard;
    }

    public List<BaseCardData> GetCardsOfCharacter(string charId)
    {
        var targetCards = AllCardContainers.Find(x => x.GetCharId() == charId);

        if (targetCards == null)
        {
            CustomDebug.LogWarning("Did not find  character with id: " + charId, Categories.Data.Cards);
            return null;
        }

        return targetCards.GetAllImplementedCards();
    }
}
