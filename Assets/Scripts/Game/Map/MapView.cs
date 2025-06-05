using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public Action<MapNode> OnNodeClicked;
    public Action OnResetClicked;
    
    [SerializeField] private Transform m_NodesContainer;
    [SerializeField] private MapNodeDisplay m_NodeDisplayPrefab;
    [SerializeField] private Material m_lineMaterial;
    
    Dictionary<int, List<MapNodeDisplay>> m_nodesByDepth = new Dictionary<int, List<MapNodeDisplay>>();

    private MapLinkedList m_map;
    
    private const float m_lineWidth = 0.2f;
    

    

    public void DisplayMap(MapLinkedList map)
    {
        ClearAllNodes();
        m_map = map;
        DisplayAllNodes(map.Head);
        ConnectAllNodes();
    }

    private void ClearAllNodes()
    {
        m_nodesByDepth.Clear();
        foreach (Transform child in m_NodesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    private MapNodeDisplay DisplayAllNodes(MapNode node, int depth = 0)
    {
        if (node == null)
        {
            return null;
        }
        
        // current node
        MapNodeDisplay nodeDisplay = GenerateNodeDisplay(node);



        // add to dict based on depth
        if (!m_nodesByDepth.ContainsKey(depth))
        {
            m_nodesByDepth.Add(depth, new List<MapNodeDisplay>());
        }
        m_nodesByDepth[depth].Add(nodeDisplay);
        
        // display all children nodes recursively 
        foreach (var child in node.Children)
        {
            MapNodeDisplay childDisplay = DisplayAllNodes(child, depth + 1);
            nodeDisplay.ConnectedNodes.Add(childDisplay);
            
            //draw line to child
            //DrawLine(nodeDisplay.gameObject, childDisplay.gameObject, Color.blue);
        }
        
        return nodeDisplay;
    }

    public void ConnectAllNodes()
    {
        for (var i = 0; i < m_nodesByDepth.Keys.Count; i++)
        {
            foreach (MapNodeDisplay mapNodeDisplay in m_nodesByDepth[i])
            {
                foreach (MapNodeDisplay childNode in mapNodeDisplay.ConnectedNodes)
                {
                    StartCoroutine(DrawLine(mapNodeDisplay.gameObject, childNode.gameObject, Color.blue));
                }
            }
        }
    }

    private IEnumerator DrawLine(GameObject start, GameObject end, Color color)
    {
        yield return new WaitForSeconds(0.1f);
        
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(start.transform);
        lineObj.transform.localPosition = Vector3.zero;
        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        
        RectTransform startRect = start.GetComponent<RectTransform>(); 
        RectTransform endRect = end.GetComponent<RectTransform>(); 
        
        Vector3 worldStart = startRect.TransformPoint(startRect.position);
        Vector3 worldEnd = endRect.TransformPoint(endRect.position);

        // worldStart.z = 0;
        // worldEnd.z = 0;
        
        line.positionCount = 2;
        line.SetPosition(0, worldStart);
        line.SetPosition(1, worldEnd);
        
        line.startWidth = m_lineWidth;
        line.endWidth = m_lineWidth;
        
        line.material = m_lineMaterial;
        line.startColor = color;
        line.endColor = color;
    }

    private MapNodeDisplay GenerateNodeDisplay(MapNode node)
    {
        MapNodeDisplay nodeDisplay = Instantiate(m_NodeDisplayPrefab, m_NodesContainer);
        
        nodeDisplay.Configure(node, node.GetImage());
        nodeDisplay.OnClick += NodeClicked;
        
        return nodeDisplay;
    }

    private void NodeClicked(MapNode node)
    {
        if (!m_map.SelectableNodes.Contains(node))
        {
            Debug.Log("node is not selectable");
            return;
        }
        
        // Debug.Log("node clicked: " + node.GetType().Name);
        OnNodeClicked?.Invoke(node);
    }

    public MapNodeDisplay FindNodeDisplay(MapNode node)
    {
        foreach (var mapNodeList in m_nodesByDepth.Values)
        {
            MapNodeDisplay nodeMatch = mapNodeList.Find(t => t.Node == node);
            if (nodeMatch != null)
            {
                return nodeMatch;
            }
        }
        return null;
    }

    public void UpdateNode(MapNode node)
    {
        MapNodeDisplay nodeDisplay = FindNodeDisplay(node);
        nodeDisplay.Configure(node, node.GetImage());
    }

    public void OnResetButtonClicked()
    {
        OnResetClicked?.Invoke();
    }
}
