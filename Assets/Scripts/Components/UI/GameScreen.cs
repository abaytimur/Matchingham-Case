using DataHandler.DataModels;
using DataHandler.GameDatas.Level;
using DG.Tweening;
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
        private int _currentStarCount;

        [Inject]
        private void Construct(GameSceneEvents gameSceneEvents) => _gameSceneEvents = gameSceneEvents;

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();

        private void RegisterEvents()
        {
            _gameSceneEvents.OnLevelStart += OnLevelStart;
            _gameSceneEvents.OnLevelEnd += OnLevelEnd;
            _gameSceneEvents.OnStarsAdded += OnStarsAdded;
        }

        private void UnRegisterEvents()
        {
            _gameSceneEvents.OnLevelStart -= OnLevelStart;
            _gameSceneEvents.OnLevelEnd -= OnLevelEnd;
            _gameSceneEvents.OnStarsAdded -= OnStarsAdded;
        }


        private void OnLevelStart(LevelDataSo levelDataSo)
        {
            _currentStarCount = 0;
            SetLevelText(PlayerDataModel.Data.lastCompletedLevel + 1);
            SetStarText();
            // timerText.SetText($"Time {levelDataSo.time}");
        }

        private void OnLevelEnd(bool isLevelCompleted)
        {
            if (isLevelCompleted)
            {
                PlayerDataModel.Data.starsCollected = _currentStarCount;
            }
        }

        private void OnStarsAdded(int startsAdded)
        {
            _currentStarCount += startsAdded;
            SetStarText();
        }

        private void SetLevelText(int currentLevel) => levelText.SetText($"Level {currentLevel}");

        private void SetStarText()
        {
            starText.transform.DOScale(1.2f, .15f).OnComplete(() =>
            {
                starText.SetText($"{_currentStarCount}");
                starText.transform.DOScale(1f, .15f);
            });
        }
    }
}