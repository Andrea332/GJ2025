using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(CameraAspectRatio))]
    public class CameraAspectRatioEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
