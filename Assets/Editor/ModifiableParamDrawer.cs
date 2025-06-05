using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModifiableParam<>))]
public class ModifiableParamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty valueProp = property.FindPropertyRelative("m_value");
        EditorGUI.PropertyField(position, valueProp, label);
    }
}
