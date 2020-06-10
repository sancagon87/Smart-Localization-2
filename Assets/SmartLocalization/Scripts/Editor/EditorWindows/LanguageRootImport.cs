using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace SmartLocalization.Editor
{
    [System.Serializable]
    public class LanguageRootImport : EditorWindow
    {
        #region Members

        CSVParser.Delimiter delimiter = CSVParser.Delimiter.COMMA;
        SmartCultureInfo chosenCulture;
        EditRootLanguageFileWindow parentWindow;

        int chosenFileFormat = 0;

        static readonly string csvFileEnding = ".csv";
        static readonly string xlsFileEnding = ".xls";

        static readonly string[] availableFileFormats = { ".csv", ".xls" };

        #endregion

        #region Initialization

        void Initialize(EditRootLanguageFileWindow parentWindow)
        {
            //this.chosenCulture = chosenCulture;
            this.parentWindow = parentWindow;
            if (chosenFileFormat >= availableFileFormats.Length)
            {
                chosenFileFormat = 0;
            }
        }

        #endregion

        #region GUI Methods

        void OnGUI()
        {
            if (LocalizationWindowUtility.ShouldShowWindow())
            {
                GUILayout.Label("Update Language from file", EditorStyles.boldLabel);
                //GUILayout.Label("Language to Update: " + chosenCulture.englishName + " - " + chosenCulture.languageCode);
                chosenFileFormat = EditorGUILayout.Popup("File Format", chosenFileFormat, availableFileFormats);

                if (availableFileFormats[chosenFileFormat] == csvFileEnding)
                {
                    delimiter = (CSVParser.Delimiter)EditorGUILayout.EnumPopup("Delimiter", delimiter);
                }

                if (GUILayout.Button("Update"))
                {
                    OnUpdateClicked();
                }
            }
        }

        #endregion


        #region Event Handlers
        void OnUpdateClicked()
        {
            string file = EditorUtility.OpenFilePanel("Select Update file.", "", "");
            if (file != null && file != "")
            {
                if (availableFileFormats[chosenFileFormat] == csvFileEnding)
                {
                    UpdateFromCSV(file);
                    this.Close();
                }
                else if (availableFileFormats[chosenFileFormat] == xlsFileEnding)
                {
                    UpdateFromXLS(file);
                    this.Close();
                }
                else
                {
                    Debug.LogError("Unsupported file format! Cannot export file!");
                }
            }
            else
            {
                Debug.Log("Failed to export language");
            }
        }

        #endregion

        #region Helper Methods

        void UpdateFromCSV(string chosenUpdateFile)
        {
            UpdateLanguageFile(CSVParser.Read(chosenUpdateFile, CSVParser.GetDelimiter(delimiter)));
            /*
            if (parentWindow.translateLanguageWindow != null)
            {
                parentWindow.translateLanguageWindow.ReloadLanguage();
            }
            */
        }

        void UpdateFromXLS(string chosenUpdateFile)
        {
            var values = XLSExporter.Read(chosenUpdateFile);
            UpdateLanguageFile(values);
            /*
            if (parentWindow.translateLanguageWindow != null)
            {
                parentWindow.translateLanguageWindow.ReloadLanguage();
            }
            */
        }

        public void UpdateLanguageFile(List<List<string>> values)
        {
            int updatedKeys = 0;
            foreach (List<string> row in values)
            {
                if (row.Count != 2)
                {
                    continue;
                }
                string key = row[0].TrimStart('\r', '\n').TrimEnd('\r', '\n').Trim();
                string value = row[1];

                parentWindow.AddKey(key, value);
                /*
                if (!languageItems.ContainsKey(key))
                {
                    continue;
                }

                languageItems[key] = value;
                */
                updatedKeys++;
            }
        }

        #endregion

        #region Show Windows
        public static LanguageRootImport ShowWindow(EditRootLanguageFileWindow parentWindow)
        {
            LanguageRootImport languageUpdateWindow = (LanguageRootImport)EditorWindow.GetWindow<LanguageRootImport>("Import Root Keys");
            languageUpdateWindow.Initialize(parentWindow);

            return languageUpdateWindow;
        }
        #endregion
    }
}
