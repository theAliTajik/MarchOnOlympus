using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLinkedList
{
    public MapNode Head;
    public List<MapNode> SelectableNodes = new List<MapNode>();
    
    public void SetHead(MapNode node)
    {
        Head = node;
    }

    public void Print()
    {
        if (Head != null)
        {
            Head.PrintSelfAndChildren();
        }
    }
}