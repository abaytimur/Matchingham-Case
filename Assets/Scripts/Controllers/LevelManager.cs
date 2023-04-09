using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.GameData.Level;
using Events.External;
using Miscellaneous;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;

namespace Controllers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelDataSo> levelList = new();
        private LevelDataSo _currentLevelDataSo;
        private GameSceneEvents _gameSceneEvents;

        [Inject]
        private void Construct(GameSceneEvents gameEventsSo) => _gameSceneEvents = gameEventsSo;
        
        [Button]
        public void GetAllLevelsFromPath()
        {
            levelList.Clear();
            levelList = Helpers.GetAllLevelDataSo().ToList();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            _gameSceneEvents.OnLevelStart?.Invoke(GetCurrentLevel());
        }

        private LevelDataSo GetCurrentLevel() => _currentLevelDataSo ??= levelList[0];
    }
}