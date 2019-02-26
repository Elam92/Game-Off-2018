using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State<BattleStateInputs>
{
    private AI ai;
    private List<AIShip> aiShips;
    private BattleController controller;
    private State<BattleStateInputs> nextState;

    public AITurnState(BattleController controller, AI ai, State<BattleStateInputs> nextState)
    {
        this.controller = controller;
        this.ai = ai;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log("ENTER AITURNSTATE");
        ai.Initialize();
    }

    public override State<BattleStateInputs> Update()
    {
        if (ai.GetShips().Count <= 0 || ai.GetTargetShips().Count <= 0)
        {
            return nextState;
        }

        ai.DoActions();

        return null;
    }
}
