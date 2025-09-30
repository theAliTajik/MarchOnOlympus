using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugPrefTester : MonoBehaviour
{
    void Start()
    {
        DebugPreferences.SetCategoryEnabled(Categories.UI.Root, true);
    }
}
