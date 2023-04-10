using Components.UI;

namespace Components.StateMachine.GamesStates
{
    public class MainMenuState: GameBaseState
    {
        private readonly MainMenuScreen _mainMenuScreen;
        public MainMenuState( MainMenuScreen mainMenuScreen) => _mainMenuScreen = mainMenuScreen;
        public override void EnterState(GameStateManager gameStateManager)
        {
            _mainMenuScreen.SetLevelData();
            _mainMenuScreen.Show();
        }

        public override void ExitState(GameStateManager gameStateManager) => _mainMenuScreen.Hide();
    }
}