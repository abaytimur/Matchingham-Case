using System.Collections;
using Components.StateMachine.GamesStates;
using Components.UI;
using DataHandler.GameDatas.Level;
using Events.External;
using UnityEngine;
using Zenject;

namespace Components.StateMachine
{
    public class GameStateManager : MonoBehaviour
    {
        private GameSceneEvents _gameSceneEvents;
        private GameBaseState _currentState;

        private LoadingState _loadingState;
        private MainMenuState _mainMenuState;
        private GameState _gameState;
        private LevelEndState _levelEndState;

        [SerializeField] private bool useUpdates;

        private LoadingScreen _loadingScreen;
        private MainMenuScreen _mainMenuScreen;
        private GameScreen _gameScreen;
        private EndGameScreen _endGameScreen;

        [Inject]
        private void Construct(LoadingScreen loadingScreen, MainMenuScreen mainMenuScreen, GameScreen gameScreen,
            EndGameScreen endGameScreen, GameSceneEvents gameSceneEvents)
        {
            _loadingScreen = loadingScreen;
            _mainMenuScreen = mainMenuScreen;
            _gameScreen = gameScreen;
            _endGameScreen = endGameScreen;
            _gameSceneEvents = gameSceneEvents;
        }

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();
        private void RegisterEvents() => _gameSceneEvents.OnLevelStart += OnLevelStart;
        private void UnRegisterEvents() => _gameSceneEvents.OnLevelStart -= OnLevelStart;
        private void OnLevelStart(LevelDataSo level) => ChangeState(_gameState);

        private void Awake() => InitializeStates();

        private void Start()
        {
            _currentState = _loadingState;
            _currentState.EnterState(this);

            float randomFakeLoadTime = Random.Range(2, 4);
            StartCoroutine(ChangeStateCoroutine(_mainMenuState, randomFakeLoadTime));
        }

        private void InitializeStates()
        {
            _loadingState = new LoadingState(_loadingScreen);
            _mainMenuState = new MainMenuState(_mainMenuScreen);
            _gameState = new GameState(_gameScreen);
            _levelEndState = new LevelEndState(_endGameScreen);
        }

        private void Update()
        {
            if (!useUpdates) return;
            _currentState.UpdateState(this);
        }

        private void ChangeState(GameBaseState newState)
        {
            if (newState == _currentState) return;

            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        private IEnumerator ChangeStateCoroutine(GameBaseState newState, float delay = 2)
        {
            yield return new WaitForSeconds(delay);
            ChangeState(newState);
        }
    }
}