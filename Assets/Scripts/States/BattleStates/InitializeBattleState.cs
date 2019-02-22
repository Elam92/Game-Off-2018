using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBattleState : State<BattleStateInputs>
{
    private List<Ship> playerShips;
    private BattleController controller;
    private readonly State<BattleStateInputs> nextState;


    public InitializeBattleState(BattleController controller, List<Ship> playerShips, State<BattleStateInputs> nextState)
    {
        this.controller = controller;
        this.playerShips = playerShips;
        this.nextState = nextState;
    }

    public override State<BattleStateInputs> Update()
    {
        if(InitializeCombat())
        {
            return nextState;
        }

        return null;
    }

    private bool InitializeCombat()
    {
        Debug.Log("INITIALIZING COMBAT");
        return true;
    }
}
