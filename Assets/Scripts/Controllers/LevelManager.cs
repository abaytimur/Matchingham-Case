using System.Collections.Generic;
using System.Linq;
using Data.GameData.Level;
using Events.External;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Controllers
{
    [UsedImplicitly]
    public class LevelManager : IInitializable
    {
        private List<LevelDataSo> _levelList;
        private readonly GameSceneEvents _gameSceneEvents;
        private LevelDataSo _currentLevelDataSo;

        [Inject]
        private LevelManager(GameSceneEvents gameEventsSo, List<LevelDataSo> levelList)
        {
            _gameSceneEvents = gameEventsSo;
            _levelList = levelList.ToList();
            Debug.LogError($"111111111");
        }

        public void Initialize()
        {
            Debug.LogError("2222222222");
                
            _gameSceneEvents.OnLevelStart?.Invoke(GetCurrentLevel());
        }

        private LevelDataSo GetCurrentLevel()
        {
            if (_currentLevelDataSo is not null)
            {
                return _currentLevelDataSo;
            }

            if (_levelList is not null && _levelList.Count > 0)
            {
                _currentLevelDataSo = _levelList[0];
            }
            else
            {
                Debug.LogError("There are no levels to lead.");
                return null;
            }

            return _currentLevelDataSo;
        }
    }
}