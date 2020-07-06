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
        UIController.Instance.ShowShipStats(ship);
        UIController.Instance.SetSelectedShip(ship);
    }

    public override void OnStateExit()
    {
        Node node = ship.GetCurrentNode();
        GameGrid.UpdateNodeState(node, GameGrid.NodeStates.Normal);
    }

    public override State<ShipStateInputs> Update()
    {
        return nextState;
    }
}
