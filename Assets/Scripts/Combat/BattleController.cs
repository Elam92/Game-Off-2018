using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    private State currentState;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentState.Tick();
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;
        gameObject.name = "Cube - " + state.GetType().Name;

        if (currentState != null)
            currentState.OnStateEnter();
    }
}
