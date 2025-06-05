using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericData<T> : ScriptableObject where T : ScriptableObject
{
    private static bool s_initialized = false;

    private static T s_instance;
    public static T Instance
    {
        get 
        {
            if (!s_initialized)
            {
                s_instance = Resources.Load<T>(typeof(T).ToString());
                if (s_instance == null)
                {
                    Debug.Log("generic data of type: " + typeof(T).ToString() + " could not be loaded.");
                }
                else
                {
                    s_initialized = true;
                }
            }

            return s_instance; 
        }
    }
}