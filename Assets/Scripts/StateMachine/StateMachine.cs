
public class StateMachine
{
    private State _currentState;

    public void SetState(State newState)
    {
        if (_currentState != null)
            _currentState.OnExit();

        _currentState = newState;

        if (_currentState != null)
            _currentState.OnEnter();
    }

    public void Update()
    {
        if (_currentState != null)
            _currentState.OnUpdate();
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}
