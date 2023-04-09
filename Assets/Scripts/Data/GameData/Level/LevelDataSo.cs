using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.GameData.Level
{
    [CreateAssetMenu(fileName = "LevelDataSo", menuName = "Game/Level Data")]
    public class LevelDataSo : SerializedScriptableObject
    {
        [BoxGroup("Level", false), GUIColor(0.8f, 1f, 0.1f)]
        public int level;

        [Tooltip(
            "The spawn number is multiplied by 3 and spawns that amount of items when the game starts. " +
            "So if you want to spawn 3 items, set the spawn number to 1.")]
        [BoxGroup("Level", false)]
        [GUIColor(0.3f, 0.8f, 0.8f)]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine,
            KeyLabel = "Item Name", ValueLabel = "Spawn Number")]
        public Dictionary<string, int> ItemNameAndSpawnNumber = new();
    }
}