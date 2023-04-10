using System.Collections.Generic;
using System.Linq;
using DataHandler.GameDatas.Level;
using Events.External;
using JetBrains.Annotations;
using UnityEditor.VersionControl;
using UnityEngine;
using Zenject;
using Task = System.Threading.Tasks.Task;

namespace Controllers
{
    [UsedImplicitly]
    public class LevelManager : IInitializable
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

        public async void Initialize()
        {
            await Task.Delay(2000);
                
            _gameSceneEvents.OnLevelStart?.Invoke(GetCurrentLevel());
        }

        private LevelDataSo GetCurrentLevel()
        {
            if (_currentLevelDataSo is not null)
            {
                Debug.LogError("1");

                return _currentLevelDataSo;
            }

            if (_levelList is not null && _levelList.Count > 0)
            {
                Debug.LogError("2");

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