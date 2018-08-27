using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace UnityCollection
{
    [CustomPropertyDrawer(typeof(LocalizationEntry))]
    public class LocalizationEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var currentLanguageCode = property
                .FindPropertyRelative("languageCode")
                .stringValue;
            var currentCulture = CultureInfo
                .GetCultureInfo(currentLanguageCode);
            
            EditorGUILayout.BeginVertical("Box");

            if (EditorGUILayout.DropdownButton(new GUIContent(currentCulture.NativeName), FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

                foreach (var culture in cultures)
                {
                    var entry = new DropdownEntry(property, culture);
                    menu.AddItem(new GUIContent(culture.NativeName), culture.Name == currentLanguageCode, OnSelectCulture, entry);
                }

                menu.DropDown(GUILayoutUtility.GetLastRect());
                menu.ShowAsContext();
            }

            EditorGUILayout.PropertyField(property.FindPropertyRelative("content"), GUIContent.none);

            EditorGUILayout.EndVertical();
        }

        private void OnSelectCulture(object entry)
        {
            var property = ((DropdownEntry) entry).property;
            var culture = ((DropdownEntry) entry).culture;

            property.FindPropertyRelative("languageCode").stringValue = culture.Name;
            property.serializedObject.ApplyModifiedProperties();
        }

        [Serializable]
        private struct DropdownEntry
        {
            public SerializedProperty property;
            public CultureInfo culture;

            public DropdownEntry(SerializedProperty property, CultureInfo culture)
            {
                this.property = property;
                this.culture = culture;
            }
        }
    }
}
