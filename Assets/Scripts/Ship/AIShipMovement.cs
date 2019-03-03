using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipMovement : ShipMovement {

    List<Node> pathToTarget;

    public void FindPath(Node currentNode, Node target)
    {
        pathToTarget = GameGrid.FindPath(currentNode, target);

        Color32 rndColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        for (int i = 0; i < pathToTarget.Count; i++)
        {
            pathToTarget[i].SetColour(rndColour);
        }
    }

}
