using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public static class JsonHelper
{
    public static bool Save<T>(T saveObject, string path)
    {
        string json = JsonUtility.ToJson(saveObject);
        
        try
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save JSON: {e.Message}");
            return false;
        }
    }

    public static T Load<T>(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("File does not Exist");
            return default(T);
        }
        
        string fileContent = File.ReadAllText(path);
        T saveObject = JsonUtility.FromJson<T>(fileContent);
        
        return saveObject;
    }
    
    public static bool SaveAdvanced<T>(T saveObject, string path)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new DefaultContractResolver
            {
                DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            }
        };

        string json = JsonConvert.SerializeObject(saveObject, settings);
        
        try
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save JSON: {e.Message}");
            return false;
        }
    }
    
    public static T LoadAdvanced<T>(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("File does not Exist");
            return default(T);
        }
        
        string fileContent = File.ReadAllText(path);
        
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new DefaultContractResolver
            {
                DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            }
        };

        T saveObject = JsonConvert.DeserializeObject<T>(fileContent, settings);
        
        return saveObject;
    }
}
