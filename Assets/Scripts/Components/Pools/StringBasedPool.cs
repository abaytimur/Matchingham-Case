using System.Collections.Generic;
using Abstract;
using UnityEngine;

namespace Components.Pools
{
    public class StringBasedPool : MonoBehaviour
    {
        [SerializeField] private List<PoolData> pools;
        private Dictionary<string, List<GameObject>> _poolDictionary;
      
        private void Start()
        {
            _poolDictionary = new Dictionary<string, List<GameObject>>();

            for (int i = 0, size = pools.Count; i < size; i++)
            {
                List<GameObject> objectPool = new List<GameObject>();

                for (int k = 0, poolSize = pools[i].size; k < poolSize; k++)
                {
                    GameObject obj = Instantiate(pools[i].prefab, transform);
                    obj.SetActive(false);
                    objectPool.Add(obj);
                }

                _poolDictionary.Add(pools[i].tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string poolTag)
        {
            if (!_poolDictionary.ContainsKey(poolTag))
                return null;

            for (int i = 0; i < _poolDictionary[poolTag].Count; i++)
            {
                GameObject objectToSpawn = _poolDictionary[poolTag][i];

                if (objectToSpawn.activeInHierarchy == false)
                {
                    objectToSpawn.SetActive(true);

                    if (_poolDictionary[poolTag][i].TryGetComponent(out IPoolObject iPoolableObject))
                    {
                        iPoolableObject.OnObjectSpawn();
                    }

                    return objectToSpawn;
                }
            }

            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i].tag == poolTag)
                {
                    GameObject objectToSpawn = Instantiate(pools[i].prefab, transform);
                    objectToSpawn.SetActive(true);
                    _poolDictionary[poolTag].Add(objectToSpawn);

                    if (objectToSpawn.TryGetComponent(out IPoolObject iPoolableObject))
                    {
                        iPoolableObject.OnObjectSpawn();
                    }

                    return objectToSpawn;
                }
            }

            return null;
        }
    }
}