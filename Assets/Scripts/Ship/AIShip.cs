using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIShip : Ship, IPointerClickHandler
{
    public Ship target;
    private List<Node> pathToTarget;

    protected override void Start()
    {
        var shipIdleState = new ShipIdleState(this);
        var shipAttackState = new ShipAIAttackState(this, shipIdleState);
        var shipMoveState = new ShipAIMoveState(this, shipAttackState);
        var shipSelectedState = new ShipAISelectedState(this, shipMoveState);
        var shipIsHitState = new ShipIsHitState(this, shipIdleState);

        shipIdleState.AddTransition(ShipStateInputs.Selected, shipSelectedState);
        shipIdleState.AddTransition(ShipStateInputs.IsHit, shipIsHitState);

        shipSelectedState.AddTransition(ShipStateInputs.Attack, shipAttackState);
        shipSelectedState.AddTransition(ShipStateInputs.Move, shipMoveState);
        shipSelectedState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        shipIsHitState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        stateMachine = new StateMachine<ShipStateInputs>(shipIdleState);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(transform.name);
        if (BattleController.Instance.IsPlayerTurn())
        {
            UIController.Instance.ShowShipStats(this);
        }
    }

    // Check if target is in within weapon range. If it is, check if there are obstacles in the way.
    public bool HasTargetInRange(int nodesToTarget)
    {
        Vector2 origin = currentNode.transform.position;
        Vector2 direction = (Vector2) target.GetCurrentNode().transform.position - origin;

        return shipWeapon.HasTargetInRange(nodesToTarget, origin, direction);
    }

    public void ShipActivated()
    {
        stateMachine.Transition(ShipStateInputs.Selected);
    }

    public void ShipMove()
    {
        stateMachine.Transition(ShipStateInputs.Move);
    }

    public void ShipAttack()
    {
        stateMachine.Transition(ShipStateInputs.Attack);
    }
}
