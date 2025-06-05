using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Olympus/MapData")]
public class MapData : ScriptableObject
{
    public enum MapNodefType
    {
        START,
        FIGHT,
        STORE,
        EVENT
    }

    [Serializable]
    public struct MapNodeData
    {
        public string clientId;
        public MapNodefType type;
        public string data;
        public string[] nodes;
    }

    public string mapName;
    public MapNodeData[] nodes;
}
