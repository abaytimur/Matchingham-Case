using System.Collections.Generic;
using Components.Items;
using Components.Pools;
using Data.GameData.Level;
using Events.External;
using Miscellaneous;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class SpawnManager : MonoBehaviour
    {
        private readonly List<MatchItem> _itemList = new();

        private GameSceneEvents _gameSceneEvents;
        private StringBasedPool _stringBasedPool;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float rectWidth = 4.25f;
        [SerializeField] private float rectHeight = 7f;
        [SerializeField] private float spawnHeight = 1f;

        [Inject]
        private void Construct(GameSceneEvents gameEventsSo, StringBasedPool stringBasedPool)
        {
            _gameSceneEvents = gameEventsSo;
            _stringBasedPool = stringBasedPool;
        }

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();
        private void RegisterEvents() => _gameSceneEvents.OnLevelStart += OnLevelStart;
        private void UnRegisterEvents() => _gameSceneEvents.OnLevelStart -= OnLevelStart;

        // Level Initialization
        private void OnLevelStart(LevelDataSo levelDataSo)
        {
            _itemList.Clear();

            for (int i = 0; i < levelDataSo.itemDataList.Count; i++)
            {
                for (int j = 0; j < levelDataSo.itemDataList[i].spawnNumber * Helpers.ItemNumberToMatch; j++)
                {
                    var temporaryItem = _stringBasedPool.SpawnFromPool(levelDataSo.itemDataList[i].itemName);
                    if (temporaryItem is null)
                    {
                        Debug.LogError("Item is null!");
                        continue;
                    }

                    if (temporaryItem.TryGetComponent(out MatchItem matchItem))
                    {
                        _itemList.Add(matchItem);
                    }
                }
            }

            SpawnItemsOnRandomPositions(_itemList);
        }

        private void SpawnItemsOnRandomPositions(List<MatchItem> itemList)
        {
            // Iterate through the matchItems list
            foreach (MatchItem item in itemList)
            {
                // Generate random x and z coordinates within the rectangle borders
                float randomX = Random.Range(-rectWidth / 2, rectWidth / 2);
                float randomZ = Random.Range(-rectHeight / 2, rectHeight / 2);

                // Create a new Vector3 for the spawn position
                Vector3 spawnPosition = new Vector3(spawnPoint.position.x + randomX, spawnHeight,
                    spawnPoint.transform.position.z + randomZ);

                item.transform.position = spawnPosition;
                item.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            // Draw a wire cube to represent the spawn borders
            Gizmos.DrawWireCube(spawnPoint.transform.position + new Vector3(0, spawnHeight, 0),
                new Vector3(rectWidth, 0, rectHeight));
        }
    }
}