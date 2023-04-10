using System;
using Sirenix.OdinInspector;

namespace DataHandler.GameDatas.Item
{
    [Serializable]
    public class ItemSpawnData
    {
        [ValueDropdown("@Miscellaneous.Helpers.GetItemPrefabNameList()")]
        public string itemName;
        public int spawnNumber = 1;
    }
}