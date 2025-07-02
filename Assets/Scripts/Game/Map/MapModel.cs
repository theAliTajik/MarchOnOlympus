using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MapModel : MonoBehaviour
{
    public Action<MapNode> OnNodeChanged;

    private MapLinkedList m_map = new MapLinkedList();
    public MapLinkedList Map => m_map;

    private const string m_savePath = "/Map/Map.txt";
    private const int m_numOfLoops = 4;

    private void Start()
    {

        
    }

    public void GenerateMap()
    {
        // start
        StartMapNode startNode = new StartMapNode(0);
        startNode.IsComplete = true;
        
        Map.SetHead(startNode);
        MapNode previousNode = Map.Head;
        
        previousNode = GenerateOneMapLoop(previousNode);
        previousNode = GenerateCombatWaveMapLoop(previousNode);
        
        for (int i = 0; i < m_numOfLoops-2; i++)
        {
            previousNode = GenerateOneMapLoop(previousNode);
        }
        
        foreach (var startNodeChild in startNode.Children)
        {
            Map.SelectableNodes.Add(startNodeChild);
            startNodeChild.IsSelectable = true;
        }
    }

    private MapNode GenerateOneMapLoop(MapNode startNode, bool generateAdvancedShop = false)
    {
        CombatMapNode combatMapNode = new CombatMapNode(startNode.ID + 1);
        EventMapNode eventMapNode = new EventMapNode(startNode.ID + 2);
        
        startNode.AddChild(combatMapNode);
        combatMapNode.AddChild(eventMapNode);
        
        if (generateAdvancedShop)
        {
            AdvancedShopMapNode advancedShopMapNode = new AdvancedShopMapNode(startNode.ID + 3);
            eventMapNode.AddChild(advancedShopMapNode);
            return advancedShopMapNode;
        }
        
        ShopMapNode shopMapNode = new ShopMapNode(startNode.ID + 3);
        eventMapNode.AddChild(shopMapNode);
        return shopMapNode;
    }
    
    
    private MapNode GenerateCombatWaveMapLoop(MapNode startNode, bool generateAdvancedShop = true)
    {
        CombatWaveMapNode combatMapNode = new CombatWaveMapNode(startNode.ID + 1);
        EventMapNode eventMapNode = new EventMapNode(startNode.ID + 2);
        
        startNode.AddChild(combatMapNode);
        combatMapNode.AddChild(eventMapNode);
        
        if (generateAdvancedShop)
        {
            AdvancedShopMapNode advancedShopMapNode = new AdvancedShopMapNode(startNode.ID + 3);
            eventMapNode.AddChild(advancedShopMapNode);
            return advancedShopMapNode;
        }
        
        ShopMapNode shopMapNode = new ShopMapNode(startNode.ID + 3);
        eventMapNode.AddChild(shopMapNode);
        return shopMapNode;
    }

    public void SaveMap()
    {
        JsonHelper.SaveAdvanced(Map, Application.persistentDataPath + m_savePath);
    }
    
    public bool LoadMap()
    {
        MapLinkedList loadedMap = JsonHelper.LoadAdvanced<MapLinkedList>(Application.persistentDataPath + m_savePath);
        
        if (loadedMap == null || loadedMap?.Head == null)
        {
            return false;
        }
        
        m_map = loadedMap;
        
        // rebuild selectable nodes to reference the same object
        // Traverse all nodes
        m_map.SelectableNodes.Clear();
        TraverseAndCollectSelectableNodes(m_map.Head);

        return true;
    }

    private void TraverseAndCollectSelectableNodes(MapNode node)
    {
        if (node == null)
            return;

        if (node.IsSelectable)
        {
            m_map.SelectableNodes.Add(node);
        }

        foreach (var child in node.Children)
        {
            TraverseAndCollectSelectableNodes(child);
        }
    }


    public void LoadFromData(MapData mapData)
    {
        m_map = MapDataAdapter.Import(mapData);
    }

    public void ClearMap()
    {
        m_map = new MapLinkedList();
    }
}