using UnityEngine;

namespace Components.Pools
{
    [System.Serializable]
    public class PoolData
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
}