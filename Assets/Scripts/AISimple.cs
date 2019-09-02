using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISimple : AI
{
    private System.Random random = new System.Random();
    private AIShip activeShip;
    private int index;

    public AISimple(List<AIShip> ownShips, List<Ship> targetShips)
    {
        this.ownShips = ownShips;
        this.targetShips = targetShips;
        activeShip = null;
        index = 0;
        foreach (AIShip ship in ownShips)
        {
            ShipHealth shipHealth = ship.GetComponent<ShipHealth>();
            shipHealth.OnDeath += RemoveShipFromList;
        }

        foreach(Ship ship in targetShips)
        {
            ShipHealth shipHealth = ship.GetComponent<ShipHealth>();
            shipHealth.OnDeath += RemoveEnemyShipFromList;
        }
    }

    public override void Initialize()
    {
        if (ownShips.Count > 0)
        {
            Shuffle(ownShips);
        }
        activeShip = null;
        index = 0;
    }

    public override void DoActions()
    {
        if (activeShip == null)
        {
            if (index < ownShips.Count)
            {
                activeShip = ownShips[index];
                Debug.Log("DOING ACTION FOR: " + activeShip.name);

                activeShip.target = GetClosestTarget(activeShip);
                activeShip.ShipActivated();
                index++;
            }
        }
        else if (activeShip.turnFinished)
        {
            activeShip = null;
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = ownShips.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Shooting.
    private Ship GetClosestTarget(AIShip ship)
    {
        if (targetShips.Count <= 0)
        {
            return null;
        }

        Node currentNode = ship.GetCurrentNode();
        Ship targetShip = targetShips[0];

        List<Node> pathToTarget = GameGrid.FindPath(currentNode, targetShip.GetCurrentNode());

        for (int i = 1; i < targetShips.Count; i++)
        {
            Ship anotherTarget = targetShips[i].GetComponent<Ship>();

            List<Node> aPath = GameGrid.FindPath(currentNode, anotherTarget.GetCurrentNode());
            if (aPath.Count < pathToTarget.Count)
            {
                pathToTarget = aPath;
                targetShip = anotherTarget;
            }
        }

        return targetShip;
    }

    private void RemoveShipFromList(object sender, EventArgs e)
    {
        ShipHealth shipHealth = (ShipHealth)sender;
        AIShip removingShip = shipHealth?.GetComponent<AIShip>();
        Debug.Log(removingShip.name);
        if (removingShip != null)
        {
            ownShips.Remove(removingShip);
        }
    }

    private void RemoveEnemyShipFromList(object sender, EventArgs e)
    {
        ShipHealth shipHealth = (ShipHealth)sender;
        Ship removingShip = shipHealth?.GetComponent<Ship>();
        Debug.Log(removingShip.name);
        if (removingShip != null)
        {
            targetShips.Remove(removingShip);
        }
    }
}
