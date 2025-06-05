using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPresenter : MonoBehaviour
{
    [SerializeField] private MapModel m_mapModel;
    [SerializeField] private MapView m_mapView;
    
    
    void Start()
    {
        bool mapLoaded = m_mapModel.LoadMap();
        // bool mapLoaded = false;
        if (!mapLoaded)
        {
            Debug.Log("map did not load");
            m_mapModel.GenerateMap();
            m_mapModel.SaveMap();
        }
        else
        {
            MapNodeSaveData savedNode = MapNodeSaveManager.GetSavedNode();

            if (savedNode is { IsCompleted: true })
            {
                ChangeSelectableNodes(savedNode);
                m_mapModel.SaveMap();
            }

            // LogSelectableMapNodes();
        }
        
        //m_mapModel.Map.Print();
        
        m_mapView.DisplayMap(m_mapModel.Map);
        
        m_mapView.OnNodeClicked += OnNodeClicked;
        m_mapView.OnResetClicked += OnResetMapClicked;
        m_mapModel.OnNodeChanged += OnNodeChanged;
    }

    private void OnResetMapClicked()
    {
        m_mapModel.ClearMap();
        m_mapModel.GenerateMap();
        m_mapModel.SaveMap();
        m_mapView.DisplayMap(m_mapModel.Map);
    }

    private void LogSelectableMapNodes()
    {
        for (var i = 0; i < m_mapModel.Map.SelectableNodes.Count; i++)
        {
            Debug.Log("selectable node: " + m_mapModel.Map.SelectableNodes[i].GetType().Name);
        }
        Debug.Log("map did load");
    }

    private void ChangeSelectableNodes(MapNodeSaveData savedNode)
    {
                    
        for (var i = 0; i < m_mapModel.Map.SelectableNodes.Count; i++)
        {
                
            if (m_mapModel.Map.SelectableNodes[i].ID != savedNode.ID)
            {
                m_mapModel.Map.SelectableNodes[i].IsSelectable = false;
                m_mapModel.Map.SelectableNodes.Remove(m_mapModel.Map.SelectableNodes[i]);
                continue;
            }
            
            m_mapModel.Map.SelectableNodes[i].IsComplete = true;
            m_mapModel.Map.SelectableNodes[i].IsSelectable = false;
                    
            foreach (MapNode mapNode in m_mapModel.Map.SelectableNodes[i].Children)
            {
                mapNode.IsSelectable = true;
                m_mapModel.Map.SelectableNodes.Add(mapNode);
            }
            
            m_mapModel.Map.SelectableNodes.Remove(m_mapModel.Map.SelectableNodes[i]);
        }
    }

    private void OnNodeChanged(MapNode node)
    {
        m_mapView.UpdateNode(node);
    }

    private void OnDestroy()
    {
        m_mapView.OnNodeClicked -= OnNodeClicked;
    }

    
    
    public void OnNodeClicked(MapNode node)
    {
        if (node.IsComplete)
        {
            return;
        }
        
        MapNodeSaveManager.SaveNodeEntered(node.ID);
        node.IsVisited = true;
        
        m_mapModel.SaveMap();
        
        node.OnClick();
    }

    public void DevMode()
    {
        MarkNodeAndChildrenSelectable(m_mapModel.Map.Head); 
        m_mapView.DisplayMap(m_mapModel.Map);
    }

    private void MarkNodeAndChildrenSelectable(MapNode node)
    {
       m_mapModel.Map.SelectableNodes.Add(node);
       foreach (MapNode childNode in node.Children)
       {
          MarkNodeAndChildrenSelectable(childNode); 
       }
    }
}
