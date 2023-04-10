using Components.UI;

namespace Components.StateMachine.GamesStates
{
    public class GameState : GameBaseState
    {
        private readonly GameScreen _gameScreen;
        public GameState(GameScreen gameScreen) => _gameScreen = gameScreen;
        public override void EnterState(GameStateManager gameStateManager) => _gameScreen.Show();
        public override void ExitState(GameStateManager gameStateManager) => _gameScreen.Hide();
    }
}