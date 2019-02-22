using System.Collections.Generic;

public abstract class State<T>
{
    private Dictionary<T, State<T>> transitions = new Dictionary<T, State<T>>();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public abstract State<T> Update();

    public void AddTransition(T key, State<T> state)
    {
        transitions[key] = state;
    }

    public State<T> GetTransition(T key)
    {
        if(transitions.ContainsKey(key))
        {
            return transitions[key];
        }
        else
        {
            return null;
        }
    }
}