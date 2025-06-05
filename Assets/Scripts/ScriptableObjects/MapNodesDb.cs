using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapNodesDb", menuName = "Olympus/MapNodesDb")]
public class MapNodesDb : GenericData<MapNodesDb>
{
    [System.Serializable]
    public class MapNodeData
    {
        public string Id;
        public Sprite Sprite;
    }
    
    [SerializeField] private List<MapNodeData> MapNodes = new List<MapNodeData>();
    
    public Sprite GetSprite(string nodeId)
    {
        foreach (var entry in MapNodes)
        {
            if (entry.Id == nodeId)
                return entry.Sprite;
        }
        return null;
    }
}
