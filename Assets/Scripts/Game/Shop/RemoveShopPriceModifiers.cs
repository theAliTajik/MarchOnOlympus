#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RemoveShopPriceModifiers
{
    [MenuItem("Tools/Remove Shop Price Modifiers")]
    public static void RemoveShopModifiers()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.Log("use this while the game is playing");
            return; 
        }
        GameProgress.Instance.Data.ShopModifiers.Clear();
    }
}
#endif