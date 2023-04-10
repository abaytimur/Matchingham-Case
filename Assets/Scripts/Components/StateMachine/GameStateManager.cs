using System.Collections;
using Components.StateMachine.GamesStates;
using Components.UI;
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

        private bool _readyToChangeStates;

        [Inject]
        private void Construct(GameSceneEvents gameEventsSo, LoadingScreen loadingScreen, MainMenuScreen mainMenuScreen)
        {
            _gameSceneEvents = gameEventsSo;
            _loadingScreen = loadingScreen;
            _mainMenuScreen = mainMenuScreen;
        }

        private void OnEnable() => RegisterEvents();

        private void OnDisable()
        {
            UnRegisterEvents();
            _readyToChangeStates = false;
        }

        private void RegisterEvents() => _gameSceneEvents.OnDataLoadCompleted += OnDataLoadCompleted;
        private void UnRegisterEvents() => _gameSceneEvents.OnDataLoadCompleted -= OnDataLoadCompleted;

        private void OnDataLoadCompleted() => StartCoroutine(ChangeStateCoroutine(_mainMenuState));

        private void Awake() => InitializeStates();

        private void Start()
        {
            _readyToChangeStates = true;
            _currentState = _loadingState;
            _currentState.EnterState(this);
        }

        private void InitializeStates()
        {
            _loadingState = new LoadingState(_loadingScreen);
            _mainMenuState = new MainMenuState(_mainMenuScreen);
            _gameState = new GameState();
            _levelEndState = new LevelEndState();
        }

        private void Update()
        {
            if (!useUpdates) return;
            _currentState.UpdateState(this);
        }

        private void ChangeState(GameBaseState newState)
        {
            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        private IEnumerator ChangeStateCoroutine(GameBaseState newState)
        {
            yield return new WaitUntil(() => _readyToChangeStates);
            ChangeState(newState);
        }
    }
}