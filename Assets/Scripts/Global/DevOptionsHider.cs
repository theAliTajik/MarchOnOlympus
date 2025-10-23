using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevOptionsHider : MonoBehaviour
{
    private void Awake()
    {
        #if !DEV_BUILD
        gameObject.SetActive(false);
        #endif
    }
}
