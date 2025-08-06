
public class StateMachine
{
    public State CurrentState {  get; private set; }

    public void SetState(State newState)
    {
        if (CurrentState != null)
            CurrentState.OnExit();

        CurrentState = newState;

        if (CurrentState != null)
            CurrentState.OnEnter();
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.OnUpdate();
    }

    public State GetCurrentState()
    {
        return CurrentState;
    }
}
