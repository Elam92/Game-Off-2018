using UnityEngine;

public class ShipAttackState : State<ShipStateInputs>
{
    private Ship ship;
    private readonly State<ShipStateInputs> nextState;
    private bool hasAttacked = false;

    public ShipAttackState(Ship ship, State<ShipStateInputs> nextState)
    {
        this.ship = ship;
        this.nextState = nextState;
    }

    public override void OnStateEnter()
    {
        Debug.Log(ship.transform.name + " ENTERING ATTACK STATE");
        Node[] nodes = ship.ShowWeaponRange();
        GameGrid.UpdateNodeStates(nodes, GameGrid.NodeStates.Targetable);
    }

    public override void OnStateExit()
    {

        BattleController.SelectedShip = null;
        GameGrid.UpdateNodeStates(ship.ShowWeaponRange(), GameGrid.NodeStates.Normal);
        ship.active = false;
        ship.turnFinished = true;
        hasAttacked = false;
    }

    public override State<ShipStateInputs> Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grid"));

            if (hit.collider != null)
            {
                Node targetNode = hit.transform.GetComponent<Node>();

                if (targetNode != null && targetNode.unit != null)
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
      // Debug.Log(hasAttacked + " " + !ship.IsFiring());
        if (hasAttacked)
        {
            Debug.Log("ENTERD");
            return nextState;
        }

        return null;
    }
}
