using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace UnityCollection
{
    [CreateAssetMenu]
    public class LocalizableString : ScriptableObject
    {
        public List<LocalizationEntry> entries;

        public string GetLocalization(string languageCode)
        {
            var culture = CultureInfo.GetCultureInfo(languageCode);
            var queryForSpecificCulture = entries.Where(entry => entry.languageCode == languageCode);
            var queryForNeutralCulture = entries.Where(entry => entry.languageCode == culture.Parent.Name);

            if (queryForSpecificCulture.Any())
            {
                return queryForSpecificCulture.First().content;
            }
            
            if (queryForNeutralCulture.Any())
            {
                return queryForNeutralCulture.First().content;
            }
            else
            {
                Debug.LogErrorFormat("Localization for {0} not found", culture.EnglishName);
                return "Localization not found";
            }
        }
    }

    [Serializable]
    public struct LocalizationEntry
    {
        public string languageCode;
        public string content;

        public LocalizationEntry(string languageCode, string content)
        {
            this.languageCode = languageCode;
            this.content = content;
        }
    }
}
