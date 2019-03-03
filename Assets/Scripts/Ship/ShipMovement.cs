﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    public AudioClip moveSFX;

    [SerializeField]
    private int movementSpeed = 1;
    private bool isMoving = false;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Node[] ShowMoveRange(Node currentNode)
    {
        List<Node> neighbours = GameGrid.GetNeighbours(currentNode.gridPosition, movementSpeed, new List<Node>(), 0, false);
        return neighbours.ToArray();
    }

    public int GetMovementSpeed()
    {
        return movementSpeed;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void MoveShip(Node from, Node to)
    {
        StartCoroutine(MakeMove(from, to));
        from.unit = null;
        to.unit = transform;
        from.traversable = true;
        to.traversable = false;
    }

    IEnumerator MakeMove(Node from, Node to)
    {
        isMoving = true;
        Vector3 curPosition;
        Vector3 direction;
        List<Node> path = GameGrid.FindPath(from, to);
        Debug.Log(transform.forward);
        for (int i = 0; i < path.Count; i++)
        {
            direction = path[i].transform.position - transform.position;
            curPosition = transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, direction);
            transform.position = path[i].transform.position;

            if (audioSource != null)
            {
                if (moveSFX != null)
                {
                    audioSource.PlayOneShot(moveSFX);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        isMoving = false;
    }
}