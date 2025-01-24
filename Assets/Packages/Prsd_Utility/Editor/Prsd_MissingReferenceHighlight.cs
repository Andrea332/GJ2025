using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

    [CustomPropertyDrawer(typeof(Object), true), DrawerPriority(DrawerPriorityLevel.ValuePriority)]
    internal class Prsd_MissingReferenceHighlight : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.objectReferenceValue)
            {
                if (label != GUIContent.none)
                {
                    EditorGUI.LabelField(position, label);
                    float labelWidth = EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                    position.x += labelWidth;
                    position.width -= labelWidth;
                }
                GUI.contentColor = new Color(1.05f, 1.1f, 0.9f);
                EditorGUI.PropertyField(position, property, GUIContent.none);
                GUI.contentColor = Color.white;
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
