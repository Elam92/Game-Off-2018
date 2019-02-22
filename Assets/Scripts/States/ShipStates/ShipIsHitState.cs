using UnityEngine;

public class ShipIsHitState : State<ShipStateInputs>
{
    private Ship ship;
    private readonly State<ShipStateInputs> nextState;

    public ShipIsHitState(Ship ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log(ship.transform.name + " ENTERING ISHIT STATE");
    }

    public override State<ShipStateInputs> Update()
    {
        return nextState;
    }
}
