using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCollection
{
    public class LocalizationEditor
    {
        [MenuItem("Localization/Import from CSV file...")]
        public static void ImportFromCSV()
        {
            var importPath = EditorUtility.OpenFilePanel("Import localized strings", "", "csv");
            var languageCodes = new List<string>();
            var contentEntries = new List<List<string>>();

            using (StreamReader file = new StreamReader(importPath))
            {
                languageCodes.AddRange(file.ReadLine().Split(';'));

                while (!file.EndOfStream)
                {
                    contentEntries.Add(file.ReadLine().Replace("\"", "").Split(';').ToList());
                }
            }

            foreach (var contentEntry in contentEntries)
            {
                var availableAssetGuids = AssetDatabase.FindAssets(contentEntry[0] + " t:LocalizableString");

                if (availableAssetGuids.Any())
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(availableAssetGuids[0]);
                    var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(LocalizableString)) as LocalizableString;
                    var entries = new List<LocalizationEntry>();

                    for (var i = 0; i < contentEntry.Count; i++)
                    {
                        entries.Add(new LocalizationEntry(languageCodes[i], contentEntry[i]));
                    }

                    asset.entries = entries;
                    
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    var asset = ScriptableObject.CreateInstance<LocalizableString>();
                    var entries = new List<LocalizationEntry>();

                    for (var i = 0; i < contentEntry.Count; i++)
                    {
                        entries.Add(new LocalizationEntry(languageCodes[i], contentEntry[i]));
                    }

                    asset.entries = entries;

                    AssetDatabase.CreateAsset(asset, "Assets/Data/Strings/" + entries[0].content + ".asset");
                }
            }
        }

        [MenuItem("Localization/Export to CSV file...")]
        public static void ExportToCSV()
        {
            var exportPath = EditorUtility.SaveFilePanel("Export localized strings", "", "localization.csv", "csv");
            var assetGuids = AssetDatabase.FindAssets("t:LocalizableString");
            var languageCodes = new List<string>();
            var contentEntries = new List<List<string>>();

            GetStringsFromAssetGUIDs(assetGuids, languageCodes, contentEntries);
            WriteStringsToFile(exportPath, languageCodes, contentEntries);
        }

        private static void GetStringsFromAssetGUIDs(string[] assetGuids, List<string> languageCodes, List<List<string>> contentEntries)
        {
            foreach (var assetGuid in assetGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(LocalizableString)) as LocalizableString;

                // Only add language codes the first time
                if (languageCodes.Count == 0)
                {
                    languageCodes.AddRange(asset.entries.Select(entry => entry.languageCode));
                }

                var contentList = asset.entries.Select(entry => entry.content).ToList();

                // Wrap content entries around quotation marks
                var contentListWithQuotationMarks = new List<string>();

                foreach (var content in contentList)
                {
                    contentListWithQuotationMarks.Add("\"" + content + "\"");
                }

                contentEntries.Add(contentListWithQuotationMarks);
            }
        }

        private static void WriteStringsToFile(string path, List<string> languageCodes, List<List<string>> contentEntries)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(string.Join(";", languageCodes.ToArray()));

                foreach (var contentEntry in contentEntries)
                {
                    file.WriteLine(string.Join(";", contentEntry.ToArray()));
                }
            }
        }
    }
}
