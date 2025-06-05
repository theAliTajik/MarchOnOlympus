using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapNodeSaveData
{
    public int ID;
    public bool IsCompleted;
}

public static class MapNodeSaveManager
{
    private const string m_nodeSavePath = "/Map/NodeEntered.txt";
    private static string m_savePath = Application.persistentDataPath + m_nodeSavePath;
    
    
    public static void MarkNodeCompleted()
    {
        // Find or create the node entry
        MapNodeSaveData node = JsonHelper.LoadAdvanced<MapNodeSaveData>(m_savePath);
        
        if (node != null)
        {
            node.IsCompleted = true;
            JsonHelper.SaveAdvanced(node, m_savePath);
        }

    }

    public static void MarkNodeCompleted(string nodeType)
    {
        // Find or create the node entry
        MapNodeSaveData node = JsonHelper.LoadAdvanced<MapNodeSaveData>(m_savePath);
        
        if (node == null || node.GetType().Name != nodeType)
        {
            return;
        }
        
        node.IsCompleted = true;
        JsonHelper.SaveAdvanced(node, m_savePath);

    }

    
    public static void SaveNodeEntered(int nodeId)
    {
        MapNodeSaveData node = new MapNodeSaveData();
        node.ID = nodeId;
        JsonHelper.SaveAdvanced(node, m_savePath);
    }

    public static MapNodeSaveData GetSavedNode()
    {
        MapNodeSaveData node = JsonHelper.LoadAdvanced<MapNodeSaveData>(m_savePath);
        if (node != null)
        {
            return node;
        }
        
        return null;
    }

    
}
