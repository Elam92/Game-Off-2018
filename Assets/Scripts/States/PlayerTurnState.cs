using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{

    List<Ship> playerShips;

    public PlayerTurnState(List<Ship> playerShips)
    {
        this.playerShips = playerShips;
    }

    public override void Tick()
    {
        if(playerShips.Count <= 0)
        {

        }
    }
}
