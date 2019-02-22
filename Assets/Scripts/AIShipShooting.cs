using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipShooting : ShipShooting
{

    public Ship GetClosestTarget(Node currentNode, GameObject[] targetShips)
    {
        List<Node> shortestPath = new List<Node>();
        Ship targetShip = targetShips[0].GetComponent<Ship>();

        shortestPath = GameGrid.FindPath(currentNode, targetShip.GetCurrentNode());

        for (int i = 1; i < targetShips.Length; i++)
        {
            Ship ship = targetShips[i].GetComponent<Ship>();

            List<Node> aPath = GameGrid.FindPath(currentNode, ship.GetCurrentNode());
            if(aPath.Count < shortestPath.Count)
            {
                shortestPath = aPath;
                targetShip = ship;
            }
        }
        return targetShip;
    }
}
