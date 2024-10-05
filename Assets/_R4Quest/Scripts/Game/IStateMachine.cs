namespace R4Quest.Game
{
    public interface IStateMachine
    {
        void ChangeState(IState state);

    }
    
    public abstract class IState
    {
        public abstract void Enter();
        public abstract void Exit();
    }

    public class GameStateMachine : IStateMachine
    {
        public IState CurrentState;

        public void ChangeState(IState state)
        {
            CurrentState = state;
        }
    }
}