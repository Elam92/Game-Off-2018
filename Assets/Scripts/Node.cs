using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    // Current occupant.
    public Transform unit;
    // Is the grid moveable through?
    public bool traversable = true;
    // Is the node within a ship's movement range?
    public bool isWithinMovementRange = false;
    // The position of the node in world space.
    public Vector3 worldPosition;
    // The position of the node inside of a grid.
    public int[] gridPosition;

    // Cost from Start to this Node.
    public int gCost;
    // Cost to Goal from this Node.
    public int hCost;
    // Total Cost to get to Goal.
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    // For retracing paths.
    public Node parent;

    private SpriteRenderer spriteRen;

    private void Awake()
    {
        worldPosition = transform.position;

        spriteRen = GetComponent<SpriteRenderer>();
    }

    public void SetColour(Color32 newColour)
    {
        spriteRen.color = newColour;
    }
}
