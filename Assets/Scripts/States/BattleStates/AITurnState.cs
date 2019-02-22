using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State<BattleStateInputs>
{

    private List<AIShip> aiShips;
    private BattleController controller;
    private State<BattleStateInputs> nextState;

    public AITurnState(BattleController controller, List<AIShip> aiShips, State<BattleStateInputs> nextState)
    {
        this.controller = controller;
        this.aiShips = aiShips;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log("ENTER AITURNSTATE");
    }

    public override State<BattleStateInputs> Update()
    {
        if (aiShips.Count <= 0)
        {
            return nextState;
        }

        return null;
    }
}
