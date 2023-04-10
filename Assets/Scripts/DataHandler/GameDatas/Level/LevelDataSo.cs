using System.Collections.Generic;
using DataHandler.GameDatas.Item;
using UnityEngine;

namespace DataHandler.GameDatas.Level
{
    [CreateAssetMenu(fileName = "LevelDataSo", menuName = "Game/Level Data")]
    public class LevelDataSo : ScriptableObject
    {
        [Tooltip(
            "The spawn number is multiplied by 3 and spawns that amount of items when the game starts. " +
            "So if you want to spawn 3 items, set the spawn number to 1.")]
        public List<ItemSpawnData> itemDataList = new();
    }
}