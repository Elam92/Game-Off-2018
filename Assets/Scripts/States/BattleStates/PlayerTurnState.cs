using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State<BattleStateInputs>
{
    private List<Ship> playerShips;
    private List<AIShip> enemyShips;
    private BattleController controller;
    private State<BattleStateInputs> nextState;

    public PlayerTurnState(BattleController controller, List<Ship> playerShips, List<AIShip> enemyShips, State<BattleStateInputs> nextState)
    {
        this.controller = controller;
        this.playerShips = playerShips;
        this.enemyShips = enemyShips;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log("ENTER PLAYER'S TURN");
        Node[] shipNodes = new Node[playerShips.Count];

        for(int i = 0; i < playerShips.Count; i++)
        {
            playerShips[i].turnFinished = false;
            shipNodes[i] = playerShips[i].GetCurrentNode();
        }

        GameGrid.UpdateNodeStates(shipNodes, GameGrid.NodeStates.Selectable);
    }

    public override void OnStateExit()
    {
        Debug.Log("LEAVING PLAYERTURNSTATE");
        foreach (Ship ship in playerShips)
        {
            ship.FinishTurn();
        }
    }

    public override State<BattleStateInputs> Update()
    {
        if(enemyShips.Count <= 0)
        {
            return nextState;
        }

        return null;
    }
}
