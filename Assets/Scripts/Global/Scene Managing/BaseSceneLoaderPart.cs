using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneLoaderPart : MonoBehaviour
{
    protected virtual void Register()
    {
        if (SceneController.Instance == null)
        {
            return;
        }
        SceneController.Instance.Register(this);
    }

    protected virtual void Unregister()
    {
        if (SceneController.Instance == null)
        {
            return;
        }

        
        SceneController.Instance.Unregister(this);
    }
}
