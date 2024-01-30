using UnityEditor;
using UnityEngine;


/// <summary>
/// Property drawer for Embedded assets that we want to expose a property of
/// </summary>
[CustomPropertyDrawer(typeof(EmbeddedPropertyAttribute))]
public class EmbeddedPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
        var objectReference = position;
        objectReference.height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        EditorGUI.ObjectField(objectReference, property);

        Rect embeddedArea = new Rect(position);
        embeddedArea.y += EditorGUI.GetPropertyHeight(property);
        embeddedArea.height -= EditorGUI.GetPropertyHeight(property);

        if (property.objectReferenceValue != null)
        {
            EditorGUI.DrawRect(embeddedArea, Color.black.WithQuarterAlpha());
            EditorGUI.indentLevel += 2;

            embeddedArea.y += EditorGUIUtility.standardVerticalSpacing;
            embeddedArea.height -= EditorGUIUtility.standardVerticalSpacing;

            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty targetProperty = serializedObject.FindProperty((attribute as EmbeddedPropertyAttribute).PropertyName);
            EditorGUI.PropertyField(embeddedArea, targetProperty);

            EditorGUI.indentLevel -= 2;
            serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) +
                            EditorGUIUtility.standardVerticalSpacing;

        if (property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty targetProperty = serializedObject.FindProperty((attribute as EmbeddedPropertyAttribute).PropertyName);

            totalHeight += EditorGUI.GetPropertyHeight(targetProperty, label, true) + EditorGUIUtility.standardVerticalSpacing;
            if (targetProperty.isArray && targetProperty.isExpanded)
            {
                totalHeight += (EditorGUIUtility.singleLineHeight + targetProperty.arraySize);
            }
        }

        return totalHeight;
    }
}