// using UnityEditor;
// using UnityEngine;
//
// public class CardDataMigration
// {
//     private const string folderPath = "Assets/Data/Resources/CardsData";
//     
//     [MenuItem("Tools/migrate Card Energy")]
//     public static void MigrateCardEnergy()
//     {
//         string[] assetpaths = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
//
//         Debug.Log("asset paths count: + " + assetpaths.Length);
//         int migratedCount = 0;
//         foreach (string guid in assetpaths)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
//             
//             
//             if(obj is BaseCardData card)
//             {
//                 card.normalDataSet.EnergyCost.Value = card.normalDataSet.EnergyCost;
//
//                 card.stanceDataSet.EnergyCost.Value = card.stanceDataSet.EnergyCost;
//
//                 Debug.Log("changed card: " + card.Name);
//                 EditorUtility.SetDirty(card);
//                 migratedCount++;
//             }
//         }
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         Debug.Log("migrated: " + migratedCount + " cards");
//     }
// }
