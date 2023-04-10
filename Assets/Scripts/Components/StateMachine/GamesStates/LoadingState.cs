namespace Components.StateMachine.GamesStates
{
    public class LoadingState : GameBaseState
    {
        private readonly LoadingScreen _loadingScreen;
        public LoadingState(LoadingScreen loadingScreen) => _loadingScreen = loadingScreen;
        public override void EnterState(GameStateManager gameStateManager) => _loadingScreen.Show();
        public override void ExitState(GameStateManager gameStateManager) => _loadingScreen.Hide();
    }
}