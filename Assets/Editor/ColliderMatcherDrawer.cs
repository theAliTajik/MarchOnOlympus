using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColliderMatcher))]
public class ColliderMatcherDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty colliderProp = property.FindPropertyRelative("m_collider");
        EditorGUI.PropertyField(position, colliderProp, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_collider"), label, true);
    }
}