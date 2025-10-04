using System;
using System.Collections.Generic;
using Game;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public enum CardPacks
{
    NONE,
    STRIKE,
    BLOCK,
    BLEED,
    IMPALE,
    COMBO,
    STR,
    DEBUFF,
    TECH,
    PRACTICAL,
    GENIUS,
    ARROW,
    BURN,
    DISCARD,
    BUFF_REMOVE,
    IMPROVISE
}

public enum CardType
{
    NONE,
    TECH
}

public enum CardRarity
{
    NONE,
    LEGENDARY
}

public class BaseCardData : ScriptableObject, ICloneable
{
    public string Name;
    public CardRarity Rarity;
    public CardType Type;
    public CardPacks CardPack;
    [JsonIgnore]
    public Sprite Image;
    public Stance MStance;
    public bool Ingenius;

    public BaseCardDataSet normalDataSet;
    public BaseCardDataSet stanceDataSet;

    public List<BaseCardData> Upgrades = new();
    
#if UNITY_EDITOR

    [ContextMenu("add to dev testing")]
    public void AddToDevTest()
    {
        DeckTemplates.Deck devtemplate = DeckTemplates.FindById("Dev Testing");
        if (devtemplate != null)
        {
            devtemplate.CardsInDeck.RemoveAt(0);
            CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
            cardInDeck.Configure(this);
            devtemplate.CardsInDeck.Insert(0, cardInDeck);
            DeckTemplates.Save();
            Debug.Log("added card: " + Name + "to dev testing");
        }
        else
        {
            Debug.Log("did not find Dev Testing card template");
        }
    }
    
#endif
    
    
    public void SetName(string name)
    {
        Name = name;
    }

    public BaseCardDataSet GetData(Stance stance)
    {
        if (stance == Stance.NONE)
        {
            return normalDataSet;
        }
        return (stance == MStance) ? stanceDataSet : normalDataSet;
    }
    
    public BaseCardAction InstantiateAction()
    {
        Type actionType = GetActionType();
        if (actionType == null)
        {
            Debug.LogError("Failed to instantiate action: Action type is null for Card: " + Name, this);
            return null;
        }

        GameObject g = new GameObject(Name + "Action");

        #if UNITY_EDITOR
        // Use AddComponent with the determined type
        if (!typeof(BaseCardAction).IsAssignableFrom(actionType))
        {
            Debug.LogError("The type returned by GetActionType is not a BaseCardAction: " + actionType, this);
            return null;
        }
        #endif
        
        return (BaseCardAction)g.AddComponent(actionType);
    }

    protected virtual Type GetActionType()
    {
        Debug.LogError("Action prefab type not assigned for Card: " + Name, this);
        return null;
    }

    public virtual string GetDescription(bool Stance = false)
    {
        Debug.LogWarning(Name + " card does not provide descriptions");
        return null;
    }
    

    public object Clone()
    {
        BaseCardData copy = (BaseCardData)this.MemberwiseClone();
        copy.normalDataSet = this.normalDataSet?.Copy();
        copy.stanceDataSet = this.stanceDataSet?.Copy();
        return copy;
    }
}

[Serializable]
public class BaseCardDataSet
{
    public int energyCost; 
    public string description;
    public TargetType targetingType;
    public List<CardActionType> actionType;
    public bool doesPerish;
    public bool doesPerishIfNotUsed;
    
    public BaseCardDataSet Copy()
    {
        return new BaseCardDataSet
        {
            energyCost = this.energyCost,
            description = this.description,
            targetingType = this.targetingType,
            doesPerish = this.doesPerish,
            doesPerishIfNotUsed = this.doesPerishIfNotUsed 
        };
    }
}

public enum TargetType
{
    PLAYER,
    ENEMY,
    PLAYER_ENEMY
}

public enum CardActionType
{
    ATTACK,
    BLOCK,
    HEAL,
    BUFF,
    DEBUFF,
    STAT_ALTER,
    MISC
}
