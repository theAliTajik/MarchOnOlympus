using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Type = System.Type;

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

    static string myAssembly = "AllScripts";
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
            },
            
            SerializationBinder = new TypeNameSerializationBinder(myAssembly)
        };

        T saveObject = JsonConvert.DeserializeObject<T>(fileContent, settings);
        
        return saveObject;
    }
}


/// <summary>
/// A helper class to guide the deserializer to find types in their new assembly.
/// This version is more robust and handles nested generic types.
/// </summary>
public class TypeNameSerializationBinder : ISerializationBinder
{
    private readonly string _newAssemblyName;

    public TypeNameSerializationBinder(string newAssemblyName)
    {
        _newAssemblyName = newAssemblyName;
    }

    public Type BindToType(string assemblyName, string typeName)
    {
        // THE FIX IS HERE:
        // We now look for "Assembly-CSharp" inside the entire type name string,
        // which handles nested generic types.
        if (typeName.Contains("Assembly-CSharp"))
        {
            // Replace the old assembly name with the new one wherever it appears.
            typeName = typeName.Replace("Assembly-CSharp", _newAssemblyName);
        }

        // We use the original assemblyName unless it's the one we are trying to replace.
        string finalAssemblyName = assemblyName.Contains("Assembly-CSharp") ? _newAssemblyName : assemblyName;

        // Reconstruct the fully qualified name to pass to GetType.
        string fullyQualifiedName = string.Format("{0}, {1}", typeName, finalAssemblyName);

        // It's safer to use the non-throwing version of GetType and handle the null.
        return Type.GetType(fullyQualifiedName, false); // Use 'false' to avoid throwing an exception here.
        // Let the serializer handle the error if it's still not found.
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        // This part remains the same.
        assemblyName = serializedType.Assembly.FullName;
        typeName = serializedType.FullName;
    }
}