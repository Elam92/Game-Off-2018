using UnityEngine;

public class ShipIdleState : State<ShipStateInputs>
{
    private Ship ship;

    public ShipIdleState(Ship ship)
    {
        this.ship = ship;
    }

    public override void OnStateEnter()
    {
        Debug.Log("ENTER " + ship.gameObject.name + " IDLE STATE.");
    }

    public override State<ShipStateInputs> Update()
    {
        return null;
    }
}
