using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Prsd_HierarchyColor
{
    static Prsd_HierarchyColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchy;
    }

    static Color baseColor = new Color32(194, 194, 194, 255);
    static Color proColor = new Color32(56, 56, 56, 255);
    static Color highlightColor = new Color32(68, 68, 68, 255);
    static Color selectedColor = new Color32(44, 93, 135, 255);

    static void DrawHierarchy(int instanceID, Rect rect)
    {
        if (EditorUtility.InstanceIDToObject(instanceID) is GameObject o)
        {
            string name = o.name;
            int i = name.IndexOf('#');
            if (i > -1 && name.Length > i + 6 && ColorUtility.TryParseHtmlString(name.Substring(i, 7), out Color c))
            {
                Color backgroundColor = Selection.Contains(o) ? selectedColor :
                    rect.Contains(Event.current.mousePosition) ? highlightColor :
                    EditorGUIUtility.isProSkin ? proColor : baseColor;

                if (!o.activeSelf) c *= 0.7f;

                rect.x += 18f;
                rect.width -= 18f;
                EditorGUI.DrawRect(rect, backgroundColor);
                GUI.color = c;
                EditorGUI.LabelField(rect, name.Remove(i, 7), EditorStyles.boldLabel);
                GUI.color = Color.white;
            }
        }
    }
}
