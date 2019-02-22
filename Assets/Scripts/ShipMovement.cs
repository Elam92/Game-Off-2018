using System.Collections;
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

    public void MoveShip(Node currentNode, Node target)
    {
        StartCoroutine(MakeMove(currentNode, target));
        currentNode.unit = null;
        target.unit = transform;
        currentNode.traversable = true;
        target.traversable = false;

        currentNode = target;
    }

    IEnumerator MakeMove(Node current, Node target)
    {
        isMoving = true;
        Vector3 lookAt;
        Vector3 curPosition;
        List<Node> path = GameGrid.FindPath(current, target);
        for (int i = 0; i < path.Count; i++)
        {
            lookAt = path[i].transform.position - transform.position;
            curPosition = transform.position;
            curPosition.x = 0;
            transform.rotation = Quaternion.FromToRotation(curPosition, lookAt);
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
