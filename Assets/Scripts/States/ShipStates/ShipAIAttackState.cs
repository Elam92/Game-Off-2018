using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAIAttackState : State<ShipStateInputs>
{
    private AIShip ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasAttacked = false;
    private bool hasNoTargets = false;
    private (Node[] range, Node[] targets)? targetNodes;

    public ShipAIAttackState(AIShip ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log("<color=red> " + ship.transform.name + " ENTERING AI ATTACK STATE</color>");
        targetNodes = ship.ShowWeaponRange();
        if (targetNodes?.targets.Length == 0)
        {
            hasNoTargets = true;
            Debug.Log("<color=red> " + ship.transform.name + " NO TARGETS</color>");
        }
        else
        {
            GameGrid.UpdateNodeStates(targetNodes?.range, GameGrid.NodeStates.WeaponRange, node => node.isWithinWeaponRange = true);
            GameGrid.UpdateNodeStates(targetNodes?.targets, GameGrid.NodeStates.Targetable, node => node.isWithinWeaponRange = true);
            Attack();
        }
    }

    public override void OnStateExit()
    {
        ship.turnFinished = true;
        BattleController.Instance.SelectedShip = null;

        GameGrid.UpdateNodeStates(targetNodes?.range, GameGrid.NodeStates.Normal, node => node.isWithinWeaponRange = false);
        GameGrid.UpdateNodeStates(targetNodes?.targets, GameGrid.NodeStates.Normal, node => node.isWithinWeaponRange = false);


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
        Node[] targets = targetNodes?.targets;
        for (int i = 0; i < targets.Length; i++)
        {
            Ship enemyShip = targets[i].unit.GetComponent<Ship>();

            if (enemyShip != null && enemyShip == ship.target)
            {
                Debug.Log("FIRING MISSILE");
                ship.Fire(targets[i]);
                hasAttacked = true;

                if (targets[i].unit == null)
                {
                    ship.target = null;
                }
            }

        }
    }
}
