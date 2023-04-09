﻿using System.Collections.Generic;
using System.IO;
using Data.GameData.Level;
using UnityEditor;
using UnityEngine;

namespace Miscellaneous
{
    public static class Helpers
    {
        private const string ItemPrefabsFolderPath = "Assets/Prefabs/Items";
        private const string LevelDataPath =  "Assets/Scripts/Data/GameData/Level";
               
        private static readonly List<string> PrefabNames = new();

        public static List<string> GetItemPrefabNameList()
        {
            // Clear the list to avoid duplicates.
            PrefabNames.Clear();

            // Get the full path of the folder.
            string fullPath = Path.Combine(Application.dataPath,
                Helpers.ItemPrefabsFolderPath.Substring("Assets/".Length));

            // Check if the folder exists.
            if (!Directory.Exists(fullPath))
            {
                Debug.LogError("Folder not found: " + Helpers.ItemPrefabsFolderPath);
                return null;
            }

            // Get the GUIDs of all assets within the folder.
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { Helpers.ItemPrefabsFolderPath });

            // Iterate through the GUIDs and get the asset path, then extract the name and add it to the list.
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string prefabName = Path.GetFileNameWithoutExtension(assetPath);
                PrefabNames.Add(prefabName);
            }

            // Print the names of the prefabs found in the folder.
            Debug.Log("Found " + PrefabNames.Count + " prefabs in the folder.");
            foreach (string name in PrefabNames)
            {
                Debug.Log(name);
            }

            return PrefabNames;
        }
        
        public static List<LevelDataSo> GetAllLevelDataSo()
        {
            List<LevelDataSo> allKitchenData = new();

            string[] assetPaths = Directory.GetFiles(LevelDataPath, "*.asset",
                SearchOption.AllDirectories);
            foreach (string item in assetPaths)
            {
                LevelDataSo scriptableObject = AssetDatabase.LoadAssetAtPath<LevelDataSo>(item);
                if (scriptableObject != null)
                {
                    allKitchenData.Add(scriptableObject);
                }
            }

            return allKitchenData;
        }
    }
}