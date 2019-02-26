using UnityEngine;

public class ShipSelectedState : State<ShipStateInputs>
{
    private Ship ship;
    private readonly State<ShipStateInputs> nextState;

    public ShipSelectedState(Ship ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        BattleController.Instance.SelectedShip = ship;
        ship.active = true;
    }

    public override State<ShipStateInputs> Update()
    {
        return nextState;
    }
}
