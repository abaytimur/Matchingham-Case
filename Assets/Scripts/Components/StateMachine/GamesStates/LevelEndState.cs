using Components.UI;

namespace Components.StateMachine.GamesStates
{
    public class LevelEndState : GameBaseState
    {
        private readonly EndGameScreen _endGameScreen;
        public LevelEndState(EndGameScreen endGameScreen) => _endGameScreen = endGameScreen;
        public override void EnterState(GameStateManager gameStateManager) => _endGameScreen.Show();
        public override void ExitState(GameStateManager gameStateManager) => _endGameScreen.Hide();
    }
}