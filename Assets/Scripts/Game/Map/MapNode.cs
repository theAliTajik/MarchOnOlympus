using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MapNode
{
    public int ID;
    public bool IsVisited;
    public bool IsComplete;
    public bool IsSelectable;
    
    public List<MapNode> Children;
    
    protected MapNode(int id)
    {
        ID = id;
        Children = new List<MapNode>();
        
    }

    public Sprite GetImage()
    {
        string nodeName = this.GetType().Name;
        // Debug.Log("node: " + nodeName);
        Sprite image = MapNodesDb.Instance.GetSprite(nodeName);
        return image;
    }
    

    public abstract void OnClick();
    
    
    public void AddChild(MapNode child)
    {
        Children.Add(child);
    }

    public void PrintSelfAndChildren()
    {
        Debug.Log($"{GetType().Name} - ID: {ID}");
        foreach (var child in Children)
        {
            child.PrintSelfAndChildren();
        }
    }
    
    public override bool Equals(object obj)
    {
        if (obj is MapNode other)
            return this.ID == other.ID; // or compare relevant properties
        return false;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode(); // or hash of compared fields
    }
}

[System.Serializable]
public class StartMapNode : MapNode
{
    public StartMapNode() : base(0) {}
    
    public StartMapNode(int id) : base(id)
    {
    }

    public override void OnClick()
    {
        
    }
}

public class CombatMapNode : MapNode
{
    private readonly Scenes m_scene = Scenes.Game;
    public string BossName;

    public CombatMapNode() : base(0) {}
    
    public CombatMapNode(int id, string bossName) : base(id)
    {
        BossName = bossName;
    }

    public CombatMapNode(int id) : base(id)
    {
        BossName = PickRandomBoss();
    }

    private string PickRandomBoss()
    {
        int randomPick = UnityEngine.Random.Range(0, EnemiesDb.Instance.allEnemies.Length);
        string bosscClientID = EnemiesDb.Instance.allEnemies[randomPick].clientID;
        return bosscClientID;
    }

    public override void OnClick()
    {
        if (string.IsNullOrWhiteSpace(BossName))
        {
            Debug.Log("boss name empty in node: " + ID);
        }
        else
        {
            // Debug.Log("Load combat scene with boss: " + BossName);
            GameSessionParams.EnemyClientId = BossName;
            SceneController.Instance.LoadScene(m_scene);
        }
    }
}


public class CombatWaveMapNode : MapNode
{
    private readonly Scenes m_scene = Scenes.Game;
    public string WaveSetId;

    public CombatWaveMapNode() : base(0) {}
    
    public CombatWaveMapNode(int id, string waveSetId) : base(id)
    {
        this.WaveSetId = waveSetId;
    }

    public CombatWaveMapNode(int id) : base(id)
    {
        WaveSetId = PickRandomWave();
    }

    private string PickRandomWave()
    {
        int randomPick = UnityEngine.Random.Range(0, CombatWavesDb.Instance.AllCombatWaveSets.Count);
        string waveClientId = CombatWavesDb.Instance.AllCombatWaveSets[randomPick].ClientID;
        return waveClientId;
    }

    public override void OnClick()
    {
        if (string.IsNullOrWhiteSpace(WaveSetId))
        {
            Debug.Log("wave id empty in node: " + ID);
        }
        else
        {
            GameSessionParams.EnemyClientId = string.Empty;
            GameSessionParams.WaveClientId = WaveSetId;
            SceneController.Instance.LoadScene(m_scene);
        }
    }
}

public class EventMapNode : MapNode
{
    private readonly Scenes m_scene = Scenes.Event;
    private string m_eventId;

    public EventMapNode() : base(0) {}

    public EventMapNode(int id, string eventId) : base(id)
    {
        m_eventId = eventId;
    }
    
    public EventMapNode(int id) : base(id)
    {
        PickRandomEvent();
    }

    private void PickRandomEvent()
    {
        int randIndex = UnityEngine.Random.Range(0, EventsDb.Instance.EventInfos.Count);
        string eventId = EventsDb.Instance.EventInfos[randIndex].EventId;
        m_eventId = eventId;
    }

    public override void OnClick()
    {
        GameSessionParams.EventId = m_eventId;
        SceneController.Instance.LoadScene(m_scene);
    }
}

public class ShopMapNode : MapNode
{
    private readonly Scenes m_scene = Scenes.Shop;

    public ShopMapNode() : base(0) {}
    
    public ShopMapNode(int id) : base(id)
    {
    }

    public override void OnClick()
    {
        SceneController.Instance.LoadScene(m_scene);
    }
}

public class AdvancedShopMapNode : MapNode
{
    private readonly Scenes m_scenes = Scenes.AdvancedShop;
    
    public AdvancedShopMapNode() : base(0) {}
    
    public AdvancedShopMapNode(int id) : base(id)
    {
    }

    public override void OnClick()
    {
        SceneController.Instance.LoadScene(m_scenes);
    }
}