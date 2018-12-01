using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship 
{
    public Ship target;

    private AIShipMovement shipMovement;
    private ShipHealth shipHealth;
    private AIShipShooting shipShooting;

    void Awake()
    {
        shipMovement = GetComponent<AIShipMovement>();
        shipHealth = GetComponent<ShipHealth>();
        shipShooting = GetComponent<AIShipShooting>();
    }

    // Use this for initialization
    void Start()
    {

    }

    public void DoActions()
    {
        GameObject[] targetShips = GameGrid.GetShips("PlayerShip");
        int movementSpeed = shipMovement.MovementSpeed;

        // No target, find closest one.
        target = shipShooting.GetClosestTarget(currentNode, targetShips);

        List<Node> path = GameGrid.FindPath(currentNode, target.currentNode);

        Color32 rndColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        for (int i = 0; i < path.Count; i++)
        {
            path[i].SetColour(rndColour);
        }
        // If out of range, get into range.
        int index = 0;
        if (path.Count > shipShooting.weaponRange && movementSpeed > 0)
        {
            Node destination = null;
            if (movementSpeed >= path.Count)
                // Minus 2 to not be in target's position and accounting for Count.
                index = path.Count - 2;
            else if (movementSpeed < path.Count)
                index = movementSpeed - 1;

            destination = path[index];
            shipMovement.MoveSprite(currentNode, destination);
            currentNode = destination;
        }

        // If within range, fire weapon.
        if (((path.Count - 1) - index) <= shipShooting.weaponRange)
        {
            Debug.Log("FIRING MISSILE");
            Node targetNode = path[path.Count - 1];
            shipShooting.fireMissle(targetNode);
            if (targetNode.unit == null)
            {
                target = null;
            }
        }
        else
        {
            GameGrid.MovedShip();
        }
    }

    private void MoveToTarget()
    {
        shipMovement.FindPath(currentNode, target.currentNode);
    }



}
