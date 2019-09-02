using UnityEngine;
using System;

public class ShipAttackState : State<ShipStateInputs>
{
    private Ship ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasAttacked = false;
    private bool hasNoTargets = false;
    private Node[] targetNodes;

    public ShipAttackState(Ship ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        targetNodes = ship.ShowWeaponRange();
        if(targetNodes.Length == 0)
        {
            hasNoTargets = true;
        }
        else
        {
            GameGrid.UpdateNodeStates(targetNodes, GameGrid.NodeStates.Targetable, node => node.isWithinWeaponRange = true);
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
        if(hasNoTargets)
        {
            return nextState;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grid"));

            if (hit.collider != null)
            {
                Node targetNode = hit.transform.GetComponent<Node>();

                if (targetNode != null && Array.Exists(targetNodes, target => target.Equals(targetNode)))
                {
                    Ship targetShip = targetNode.unit.GetComponent<Ship>();
                    if (targetShip != null)
                    {
                        // Attack if enemy ship.
                        if (!targetShip.tag.Equals(ship.tag))
                        {
                            ship.Fire(targetNode);
                            hasAttacked = true;
                            Debug.Log("ATTACKING " + hit.transform.name);
                        }
                        // Skip Attack Action if target ship is itself.
                        else if(targetShip == ship)
                        {
                            Debug.Log("SKIP ATTACK");
                            hasAttacked = true;
                        }
                    }

                }
            }
        }

        if (hasAttacked)
        {
            return nextState;
        }

        return null;
    }
}
