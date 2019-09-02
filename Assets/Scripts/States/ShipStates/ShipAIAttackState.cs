using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAIAttackState : State<ShipStateInputs>
{
    private AIShip ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasAttacked = false;
    private bool hasNoTargets = false;
    private Node[] targetNodes;

    public ShipAIAttackState(AIShip ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log(ship.transform.name + " ENTERING AI ATTACK STATE");
        targetNodes = ship.ShowWeaponRange();
        if (targetNodes.Length == 0)
        {
            hasNoTargets = true;
        }
        else
        {
            GameGrid.UpdateNodeStates(targetNodes, GameGrid.NodeStates.Targetable, node => node.isWithinWeaponRange = true);
            Attack();
        }
    }

    public override void OnStateExit()
    {
        ship.turnFinished = true;
        BattleController.Instance.SelectedShip = null;

        GameGrid.UpdateNodeStates(targetNodes, GameGrid.NodeStates.Normal, node => node.isWithinWeaponRange = false);

        hasAttacked = false;
        hasNoTargets = false;
        targetNodes = null;
    }

    public override State<ShipStateInputs> Update()
    {
        // No valid targets, skip Attack State.
        if (hasNoTargets || hasAttacked && !ship.IsFiring())
        {
            return nextState;
        }

        return null;
    }

    private void Attack()
    {
        // If within range, fire weapon.
        for (int i = 0; i < targetNodes.Length; i++)
        {
            Ship enemyShip = targetNodes[i].unit.GetComponent<Ship>();

            if (enemyShip != null && enemyShip == ship.target)
            {
                Debug.Log("FIRING MISSILE");
                ship.Fire(targetNodes[i]);
                hasAttacked = true;

                if (targetNodes[i].unit == null)
                {
                    ship.target = null;
                }
            }

        }
    }
}
