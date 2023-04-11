using System.Collections.Generic;
using System.Linq;
using DataHandler.DataModels;
using DataHandler.GameDatas.Level;
using Events.External;
using JetBrains.Annotations;
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

        public void StartLevel()
        {
            if (_levelList is null || _levelList.Count == 0)
            {
                Debug.LogError("There are no levels to lead.");
                return;
            }

            LevelDataSo nextLevelDataSo =_levelList[ PlayerDataModel.Data.lastCompletedLevel % _levelList.Count];
            _currentLevelDataSo = nextLevelDataSo;
            
            _gameSceneEvents.OnLevelStart?.Invoke(_currentLevelDataSo);
        }

        public void PlayNextLevel()
        {
            if (_levelList is null || _levelList.Count == 0)
            {
                Debug.LogError("There are no levels to lead.");
                return;
            }

            int nextLevelIndex = (PlayerDataModel.Data.lastCompletedLevel+1) % _levelList.Count;
            LevelDataSo nextLevelDataSo =_levelList[nextLevelIndex];
            _currentLevelDataSo = nextLevelDataSo;
            
            _gameSceneEvents.OnLevelStart?.Invoke(_currentLevelDataSo);
        }

        public void PlayCurrentLevel()
        {
            if (_currentLevelDataSo == null)
            {
                Debug.LogError("There is no current level to play.");
                return;
            }

            _gameSceneEvents.OnLevelStart?.Invoke(_currentLevelDataSo);
        }
    }
}