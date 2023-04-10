using System;
using System.Collections.Generic;
using System.Linq;
using DataHandler.DataModels;
using DataHandler.GameDatas.Level;
using Events.External;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Controllers
{
    [UsedImplicitly]
    public class LevelManager 
    {
        private readonly List<LevelDataSo> _levelList;
        private readonly GameSceneEvents _gameSceneEvents;
        private LevelDataSo _currentLevelDataSo;

        [Inject]
        private LevelManager(GameSceneEvents gameEventsSo, LevelDataSo[] levelList)
        {
            _gameSceneEvents = gameEventsSo;
            _levelList = levelList.ToList();
        }

        public void StartLevel(int levelToLoad)
        {
            if (_levelList is null || _levelList.Count == 0)
            {
                Debug.LogError("There are no levels to lead.");
                return;
            }

            // Adjust levelToLoad using the modulo operator to fit within the bounds of _levelList
            int adjustedLevelToLoad = levelToLoad % _levelList.Count;
            _currentLevelDataSo = _levelList[adjustedLevelToLoad];

            _gameSceneEvents.OnLevelStart?.Invoke(GetCurrentLevel());
        }

        [Button]
        private LevelDataSo PlayNextLevel()
        {
            if (_levelList is null || _levelList.Count == 0)
            {
                Debug.LogError("There are no levels to lead.");
                return null;
            }

            int currentLevelIndex = _levelList.IndexOf(_currentLevelDataSo);

            if (currentLevelIndex < 0)
            {
                Debug.LogError("Current level not found in the level list.");
                return null;
            }

            // Get the next level using the modulo operator for wrapping around the list.
            LevelDataSo nextLevelDataSo = _levelList.ElementAtOrDefault((currentLevelIndex + 1) % _levelList.Count);


            _gameSceneEvents.OnLevelStart?.Invoke(nextLevelDataSo);

            return nextLevelDataSo;
        }

        private LevelDataSo GetCurrentLevel()
        {
            if (_currentLevelDataSo is not null)
                return _currentLevelDataSo;

            if (_levelList is null || _levelList.Count == 0)
            {
                Debug.LogError("There are no levels to lead.");
                return null;
            }

            int adjustedLevelToLoad = PlayerDataModel.Data.lastCompletedLevel % _levelList.Count;
            return _currentLevelDataSo = _levelList[adjustedLevelToLoad];
        }
    }
}