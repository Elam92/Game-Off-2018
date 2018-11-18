using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler {

    public Transform unit;  // Current occupant.
    public bool traversable = true;
    public Vector3 worldPosition;
    public int[] gridPosition;

    private SpriteRenderer spriteRen;
    private Color32 originalColour;

    private void Awake()
    {
        worldPosition = transform.position;

        spriteRen = GetComponent<SpriteRenderer>();
        originalColour = spriteRen.color;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // TODO: Get unit's Range.
        if(GameGrid.selectedNode != null)
        {
            GameGrid.selectedNode.ResetNeighbours();   
        }

        GameGrid.selectedNode = this;
        // Hard-coded value is range value.
        Node[] neighbours = GameGrid.GetNeighbours(gridPosition, 1);

        for (int i = 0; i < neighbours.Length; i++)
        {
            neighbours[i].SetColour(new Color32(0, 255, 0, 255));
        }
    }

    public void ResetNeighbours()
    {
        Node[] neighbours = GameGrid.GetNeighbours(gridPosition, 1);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if(neighbours[i].traversable)
            neighbours[i].SetColour(originalColour);
        }
    }

    private void SetColour(Color32 newColour)
    {
        spriteRen.color = newColour;
    }
}
