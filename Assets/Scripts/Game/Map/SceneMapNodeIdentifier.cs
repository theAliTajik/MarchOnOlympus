using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMapNodeIdentifier : MonoBehaviour
{

    [SerializeField] private string nodeType; 
    
    public void NotifyNodeCompleted()
    {
        if (!string.IsNullOrEmpty(nodeType))
        {
            MapNodeSaveManager.MarkNodeCompleted(nodeType);
        }
        else
        {
            Debug.LogError("Node ID is not set!");
        }
    }
}
