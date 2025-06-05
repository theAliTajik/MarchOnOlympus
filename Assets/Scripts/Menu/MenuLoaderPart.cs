using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoaderPart : BaseSceneLoaderPart
{   
    private IEnumerator Start()
    {
        // if (someCondition)
        Register();

        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        Unregister();
    }
}