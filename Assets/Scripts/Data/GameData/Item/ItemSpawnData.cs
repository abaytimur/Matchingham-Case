using System;
using Sirenix.OdinInspector;

namespace Data.GameData.Item
{
    [Serializable]
    public class ItemSpawnData
    {
        [ValueDropdown("@Miscellaneous.Helpers.GetItemPrefabNameList()")]
        public string itemName;
        public int spawnNumber = 1;
    }
}