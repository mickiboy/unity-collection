using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCollection
{
    [RequireComponent(typeof(Text))]
    public class TextLocalizer : MonoBehaviour
    {
        public LocalizableString localizableString;

        private static List<TextLocalizer> localizers = new List<TextLocalizer>();

        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
            localizers.Add(this);
        }

        private void OnDestroy()
        {
            localizers.Remove(this);
        }

        private void UpdateLocalization(string languageCode)
        {
            text.text = localizableString.GetLocalization(languageCode);
        }

        public static void UpdateLocalizations(string languageCode)
        {
            foreach (var localizer in localizers)
            {
                localizer.UpdateLocalization(languageCode);
            }
        }
    }
}
