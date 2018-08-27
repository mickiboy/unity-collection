using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCollection
{
    [CustomEditor(typeof(LocalizableString))]
    public class LocalizableStringEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var entries = serializedObject.FindProperty("entries");

            for (var i = 0; i < entries.arraySize; i++)
            {
                var entry = entries.GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(entry);

                if (GUILayout.Button("-"))
                {
                    entries.DeleteArrayElementAtIndex(i);
                }
            }

            if (GUILayout.Button("+"))
            {
                entries.InsertArrayElementAtIndex(entries.arraySize);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
