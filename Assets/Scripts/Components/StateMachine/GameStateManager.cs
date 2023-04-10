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
        private GameBaseState _currentState;

        private LoadingState _loadingState;
        private MainMenuState _mainMenuState;
        private GameState _gameState;
        private LevelEndState _levelEndState;

        [SerializeField] private bool useUpdates;

        private LoadingScreen _loadingScreen;
        private MainMenuScreen _mainMenuScreen;

        [Inject]
        private void Construct(LoadingScreen loadingScreen, MainMenuScreen mainMenuScreen)
        {
            _loadingScreen = loadingScreen;
            _mainMenuScreen = mainMenuScreen;
        }

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