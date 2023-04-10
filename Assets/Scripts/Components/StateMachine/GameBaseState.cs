namespace Components.StateMachine
{
    public abstract class GameBaseState
    {
        public abstract void EnterState(GameStateManager gameStateManager);

        public virtual void UpdateState(GameStateManager gameStateManager)
        {
        }

        public abstract void ExitState(GameStateManager gameStateManager);
    }
}