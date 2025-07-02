using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevRemoveAllPerks : MonoBehaviour
{
    public void RemoveAllPerks()
    {
        if (!PerksManager.Instance)
        {
            Debug.Log("WARNING: No perk manager instance found");
            return;
        }
        
        PerksManager.Instance.RemoveAllPerks();
    }
}
