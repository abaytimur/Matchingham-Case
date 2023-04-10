using DataHandler.DataModels;
using DataHandler.GameDatas.Level;
using Events.External;
using TMPro;
using UnityEngine;
using Zenject;

namespace Components.UI
{
    public class GameScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI starText;
        [SerializeField] private TextMeshProUGUI timerText;
        private GameSceneEvents _gameSceneEvents;
        
        [Inject]
        private void Construct(GameSceneEvents gameSceneEvents) => _gameSceneEvents = gameSceneEvents;

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();
        private void RegisterEvents() => _gameSceneEvents.OnLevelStart += OnLevelStart;
        private void UnRegisterEvents() => _gameSceneEvents.OnLevelStart -= OnLevelStart;

        private void OnLevelStart(LevelDataSo levelDataSo)
        {
            SetLevelText(PlayerDataModel.Data.lastCompletedLevel + 1);
            SetStarText(0);
            // timerText.SetText($"Time {levelDataSo.time}");
        }
        
        private void SetLevelText(int currentLevel) => levelText.SetText($"Level {currentLevel}");
        private void SetStarText(int star) => starText.SetText($"{star}");
    }
}