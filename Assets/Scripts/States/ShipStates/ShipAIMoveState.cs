using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAIMoveState : State<ShipStateInputs>
{
    private AIShip ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasMoved = false;
    private Node[] targetNodes;

    public ShipAIMoveState(AIShip ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log(ship.transform.name + " ENTERING AI MOVE STATE");
        targetNodes = ship.ShowMovementRange();
        GameGrid.UpdateNodeStates(targetNodes, GameGrid.NodeStates.Moveable, node => node.isWithinMovementRange = true);
    }

    public override void OnStateExit()
    {
        GameGrid.UpdateNodeStates(targetNodes, GameGrid.NodeStates.Normal, node => node.isWithinMovementRange = false);
        targetNodes = null;
        hasMoved = false;
    }

    public override State<ShipStateInputs> Update()
    {
        int movementSpeed = ship.GetMovementSpeed();

        if(hasMoved == false && ship.target != null)
        {
            // If out of range, get into range
            int index = 0;
            List<Node> pathToTarget = GameGrid.FindPath(ship.GetCurrentNode(), ship.target.GetCurrentNode());
            if (pathToTarget.Count > ship.GetWeaponRange() && movementSpeed > 0)
            {
                Node destination = null;
                if (movementSpeed >= pathToTarget.Count)
                    // Minus 2 to not be in target's position and accounting for Count.
                    index = pathToTarget.Count - 2;
                else if (movementSpeed < pathToTarget.Count)
                    index = movementSpeed - 1;

                destination = pathToTarget[index];
                ship.Move(destination);
                Debug.Log(ship.name + " MOVES TO: " + destination.name);
            }
        }
        hasMoved = true;

        if (hasMoved && !ship.IsMoving())
        {
            return nextState;
        }

        return null;
    }
}
