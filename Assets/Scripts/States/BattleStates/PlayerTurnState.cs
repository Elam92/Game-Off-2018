﻿using System.Collections;
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
        foreach(Ship ship in playerShips)
        {
            ship.turnFinished = false;
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("LEAVING PLAYERTURNSTATE");
        foreach (Ship ship in playerShips)
        {
            ship.turnFinished = true;
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
