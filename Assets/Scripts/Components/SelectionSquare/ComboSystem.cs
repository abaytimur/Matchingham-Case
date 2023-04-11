using DataHandler.GameDatas.Level;
using Events.External;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Components.SelectionSquare
{
    public class ComboSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshPro comboText;
        [SerializeField] private Image comboTimerImage;
        [SerializeField] [Range(4, 12)] private float timerDuration = 8f;

        private GameSceneEvents _gameSceneEvents;
        private int _currentMultiplier;
        private float _countdownTimer;
        private bool _canCountdown;

        [Inject]
        private void Construct(GameSceneEvents gameSceneEvents) => _gameSceneEvents = gameSceneEvents;

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();

        private void RegisterEvents()
        {
            _gameSceneEvents.OnLevelStart += OnLevelStart;
            _gameSceneEvents.OnLevelEnd += OnLevelEnd;
            _gameSceneEvents.OnItemsMatched += OnItemsMatched;
        }

        private void UnRegisterEvents()
        {
            _gameSceneEvents.OnLevelStart -= OnLevelStart;
            _gameSceneEvents.OnLevelEnd -= OnLevelEnd;
            _gameSceneEvents.OnItemsMatched -= OnItemsMatched;
        }

        private void OnLevelStart(LevelDataSo levelDataSo)
        {
            _currentMultiplier = 0;
            _countdownTimer = 0;
            UpdateUI();
            _canCountdown = true;
        }

        private void OnLevelEnd(bool isLevelCompleted) => _canCountdown = false;

        private void OnItemsMatched()
        {
            _currentMultiplier = (_countdownTimer > 0) ? _currentMultiplier + 1 : 1;
            _gameSceneEvents.OnStarsAdded?.Invoke(_currentMultiplier);
            _countdownTimer = timerDuration;
            UpdateUI();
        }

        private void Update()
        {
            if (!_canCountdown || _countdownTimer <= 0) return;

            _countdownTimer -= Time.deltaTime;
            UpdateUI();

            if (!(_countdownTimer <= 0)) return;
            _currentMultiplier = 1;
            UpdateUI();
        }

        private void UpdateUI()
        {
            comboText.SetText($"x{_currentMultiplier}");
            comboTimerImage.fillAmount = _countdownTimer / timerDuration;
        }
    }
}
