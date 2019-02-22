using UnityEngine;

public class ShipMoveState : State<ShipStateInputs>
{
    private Ship ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasMoved = false;

    public ShipMoveState(Ship ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log(ship.transform.name + " ENTERING MOVE STATE");
        Node[] nodes = ship.ShowMovementRange();
        GameGrid.UpdateNodeStates(nodes, GameGrid.NodeStates.Moveable, node => node.isWithinMovementRange = true);
    }

    public override void OnStateExit()
    {
        hasMoved = false;
        GameGrid.UpdateNodeStates(ship.ShowMovementRange(), GameGrid.NodeStates.Normal, node => node.isWithinMovementRange = false);
    }

    public override State<ShipStateInputs> Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grid"));

            if (hit.collider != null)
            {
                Node targetNode = hit.transform.GetComponent<Node>();

                if (targetNode != null)
                {
                    // Move to unoccupied space.
                    if (targetNode.unit == null && targetNode.isWithinMovementRange)
                    {
                        ship.Move(targetNode);
                        hasMoved = true;
                        Debug.Log("MOVE TO: " + hit.transform.name);
                    }
                    // Skip Move Action if target ship is itself.
                    else if (targetNode.unit == ship.transform)
                    {
                        Debug.Log("SKIPPING MOVE");
                        hasMoved = true;
                    }
                }
            }
        }

        if (hasMoved && !ship.IsMoving())
        {
            return nextState;
        }

        return null;
    }
}
