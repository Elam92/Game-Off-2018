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
	private Color32 moveableColour;
	private Color32 targetColour;

    // Cost from Start to this Node.
    public int gCost;
    // Cost to Goal from this Node.
    public int hCost;
    // Total Cost to get to Goal.
    public int fCost
    {
        get {
            return gCost + hCost;
        }
    }

    public Node parent;

    private void Awake()
    {
        worldPosition = transform.position;

        spriteRen = GetComponent<SpriteRenderer>();
        originalColour = spriteRen.color;
		moveableColour = new Color32 (0, 255, 0, 40);
		targetColour = new Color32 (255, 0, 0, 40);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
	{
        if (GameGrid.playerTurn == true)
        {
			FinishAction();
            GameGrid.selectedNode = this;
            CurrentAction();
        }
	}

	private void FinishAction(){
		if (GameGrid.selectedNode != null)
		{
			if (this.spriteRen.color == moveableColour && GameGrid.selectedNode.unit.GetComponent<Ship> ().moving == true) {
				GameGrid.selectedNode.ResetNeighbours ();
				GameGrid.selectedNode.unit.GetComponent<ShipMovement> ().MoveSprite (GameGrid.selectedNode, this);
                Debug.Log("UNIT: " + unit.transform.name);
			} 
			else if (this.spriteRen.color == targetColour && GameGrid.selectedNode.unit.GetComponent<Ship> ().shooting == true) 
			{
				GameGrid.selectedNode.ResetNeighbours ();
				GameGrid.selectedNode.unit.GetComponent<ShipShooting> ().fireMissle (this);
			} 
			else 
			{
				GameGrid.selectedNode.ResetNeighbours ();
			}
			//reset UI
		}
	}

	private void CurrentAction(){
		if (unit != null && unit.GetComponent<Ship>().shooting && unit.GetComponent<ShipShooting> () == true) {
			Node[] neighbours = unit.GetComponent<ShipShooting>().ShotRange(gridPosition,unit);
			if (neighbours.Length == 0) {
				GameGrid.selectedNode.unit.GetComponent<Ship>().shooting = false;
				GameGrid.selectedNode.unit.GetComponent<Ship>().activated = true;
				GameGrid.MovedShip();
				//reset ui
			}
            SetSquareType(false, neighbours);
			GameGrid.SetUI();
		}

		else if (unit != null && unit.GetComponent<ShipMovement> () == true && !unit.GetComponent<Ship>().activated && unit.tag == "PlayerShip") {
			Node[] neighbours = unit.GetComponent<ShipMovement>().MoveRange(gridPosition);
			SetSquareType (true, neighbours);
			unit.GetComponent<Ship> ().moving = true;
			GameGrid.SetUI();
		}
    }

	public void SetSquareType(bool isMove,Node[] neighbours){
		for (int i = 0; i < neighbours.Length; i++) {
			if ((neighbours [i].traversable || neighbours[i].unit == unit) && isMove) {
				neighbours [i].SetColour (moveableColour);
			}
			else if (!isMove && neighbours [i].gridPosition != gridPosition) {
				neighbours [i].SetColour (targetColour);
			}
		}
	}

    public void ResetNeighbours()
    {
		Node[] neighbours;
		if (unit != null && unit.GetComponent<ShipMovement> () == true) {
			if (unit.GetComponent<ShipMovement> ().MovementSpeed > unit.GetComponent<ShipShooting> ().weaponRange) {
				neighbours = unit.GetComponent<ShipMovement> ().MoveRange (gridPosition);
			} else {
				neighbours = unit.GetComponent<ShipShooting> ().ResetRange(gridPosition);
			}
			for (int i = 0; i < neighbours.Length; i++) {
				neighbours [i].SetColour(originalColour);
			}
		}
    }

    public void SetColour(Color32 newColour)
    {
        spriteRen.color = newColour;
    }
}
