public class StateMachine<T>
{
    private State<T> currentState;

    public StateMachine(State<T> initialState)
    {
        currentState = initialState;
    }

    public void Update()
    {
        var nextState = currentState.Update();
        ChangeState(nextState);
    }

    public void Transition(T transition)
    {
        var nextState = currentState.GetTransition(transition);
        ChangeState(nextState);
    }

    private void ChangeState(State<T> nextState)
    {
        if(nextState != null)
        {
            currentState.OnStateExit();
            nextState.OnStateEnter();
            currentState = nextState;
        }
    }
}
