using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State<BattleStateInputs>
{
    private AI ai;
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
        Debug.Log("<color=yellow>ENTER AITURNSTATE</color>");

        List<AIShip> ships = ai.GetShips();
        foreach (Ship ship in ships)
        {
            ship.turnFinished = false;
        }

        ai.Initialize();
    }

    public override void OnStateExit()
    {
        List<AIShip> ships = ai.GetShips();
        foreach (Ship ship in ships)
        {
            ship.turnFinished = true;
        }
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
