using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship 
{
    public Ship target;

    private List<Node> pathToTarget;

    public void DoActions()
    {
        GameObject[] targetShips = GameGrid.GetShips("PlayerShip");
        int movementSpeed = GetMovementSpeed();
        Node currentNode = GetCurrentNode();

        // No target, find closest one.
        target = GetClosestTarget(currentNode, targetShips);

        Debug.Log(target.transform.name);

        List<Node> path = GameGrid.FindPath(currentNode, target.GetCurrentNode());

        /*
        Color32 rndColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        for (int i = 0; i < path.Count; i++)
        {
            path[i].SetColour(rndColour);
        }
        */

        // If out of range, get into range.
        int index = 0;
        if (path.Count > GetWeaponRange() && movementSpeed > 0)
        {
            Node destination = null;
            if (movementSpeed >= path.Count)
                // Minus 2 to not be in target's position and accounting for Count.
                index = path.Count - 2;
            else if (movementSpeed < path.Count)
                index = movementSpeed - 1;

            destination = path[index];
            Move(destination);
            currentNode = destination;
        }

        // If within range, fire weapon.
        if (((path.Count - 1) - index) <= GetWeaponRange())
        {
            Debug.Log("FIRING MISSILE");
            Node targetNode = path[path.Count - 1];
            Fire(targetNode);
            if (targetNode.unit == null)
            {
                target = null;
            }
        }
        else
        {
//            GameGrid.MovedShip();
        }
    }

    private void MoveToTarget()
    {
        FindPath(GetCurrentNode(), target.GetCurrentNode());
    }

    // Moving to target.
    private void FindPath(Node currentNode, Node target)
    {
        pathToTarget = GameGrid.FindPath(currentNode, target);

        Color32 rndColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        for (int i = 0; i < pathToTarget.Count; i++)
        {
            pathToTarget[i].SetColour(rndColour);
        }
    }

    // Shooting.
    private Ship GetClosestTarget(Node currentNode, GameObject[] targetShips)
    {
        List<Node> shortestPath = new List<Node>();
        Ship targetShip = targetShips[0].GetComponent<Ship>();

        shortestPath = GameGrid.FindPath(currentNode, targetShip.GetCurrentNode());

        for (int i = 1; i < targetShips.Length; i++)
        {
            Ship ship = targetShips[i].GetComponent<Ship>();

            List<Node> aPath = GameGrid.FindPath(currentNode, ship.GetCurrentNode());
            if (aPath.Count < shortestPath.Count)
            {
                shortestPath = aPath;
                targetShip = ship;
            }
        }
        return targetShip;
    }

}
