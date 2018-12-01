using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipShooting : ShipShooting
{

    public Ship GetClosestTarget(Node currentNode, GameObject[] targetShips)
    {
        List<Node> shortestPath = new List<Node>();
        Ship targetShip = targetShips[0].GetComponent<Ship>();

        shortestPath = GameGrid.FindPath(currentNode, targetShip.currentNode);

        for (int i = 1; i < targetShips.Length; i++)
        {
            Ship ship = targetShips[i].GetComponent<Ship>();

            Debug.Log("CURRENT NODE: " + currentNode.transform.name);
            Debug.Log(ship.currentNode.transform.name);
            List<Node> aPath = GameGrid.FindPath(currentNode, ship.currentNode);
            Debug.Log("SHORTEST: " + shortestPath == null + " " + shortestPath.Count);
            Debug.Log("A PATH: " + aPath == null + " " + aPath.Count);
            if(aPath.Count < shortestPath.Count)
            {
                shortestPath = aPath;
                targetShip = ship;
            }
        }
        return targetShip;
    }
}
