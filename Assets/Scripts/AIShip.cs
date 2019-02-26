using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship 
{
    public Ship target;
    private List<Node> pathToTarget;

    protected override void Start()
    {
        var shipIdleState = new ShipIdleState(this);
        var shipAttackState = new ShipAIAttackState(this, shipIdleState);
        var shipMoveState = new ShipAIMoveState(this, shipAttackState);
        var shipSelectedState = new ShipSelectedState(this, shipMoveState);
        var shipIsHitState = new ShipIsHitState(this, shipIdleState);

        shipIdleState.AddTransition(ShipStateInputs.Selected, shipSelectedState);
        shipIdleState.AddTransition(ShipStateInputs.IsHit, shipIsHitState);

        shipSelectedState.AddTransition(ShipStateInputs.Attack, shipAttackState);
        shipSelectedState.AddTransition(ShipStateInputs.Move, shipMoveState);
        shipSelectedState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        shipIsHitState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        stateMachine = new StateMachine<ShipStateInputs>(shipIdleState);
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
