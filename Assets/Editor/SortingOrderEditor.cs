using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshRenderer))]
public class SortingLayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target MeshRenderer
        MeshRenderer meshRenderer = (MeshRenderer)target;

        // Sorting Layer
        string[] sortingLayerNames = GetSortingLayerNames();
        int currentSortingLayerIndex = GetSortingLayerIndex(meshRenderer.sortingLayerID);

        int newSortingLayerIndex = EditorGUILayout.Popup("Sorting Layer", currentSortingLayerIndex, sortingLayerNames);

        if (newSortingLayerIndex != currentSortingLayerIndex)
        {
            Undo.RecordObject(meshRenderer, "Change Sorting Layer");
            meshRenderer.sortingLayerID = SortingLayer.NameToID(sortingLayerNames[newSortingLayerIndex]);
            EditorUtility.SetDirty(meshRenderer);
        }

        // Sorting Order
        int newSortingOrder = EditorGUILayout.IntField("Sorting Order", meshRenderer.sortingOrder);

        if (newSortingOrder != meshRenderer.sortingOrder)
        {
            Undo.RecordObject(meshRenderer, "Change Sorting Order");
            meshRenderer.sortingOrder = newSortingOrder;
            EditorUtility.SetDirty(meshRenderer);
        }
    }

    private string[] GetSortingLayerNames()
    {
        int layersCount = SortingLayer.layers.Length;
        string[] sortingLayerNames = new string[layersCount];
        for (int i = 0; i < layersCount; i++)
        {
            sortingLayerNames[i] = SortingLayer.layers[i].name;
        }
        return sortingLayerNames;
    }

    private int GetSortingLayerIndex(int sortingLayerID)
    {
        SortingLayer[] layers = SortingLayer.layers;
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].id == sortingLayerID)
            {
                return i;
            }
        }
        return 0; // Default to the first layer if not found
    }
}
