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

    private void Awake()
    {
        worldPosition = transform.position;

        spriteRen = GetComponent<SpriteRenderer>();
        originalColour = spriteRen.color;
		moveableColour = new Color32 (0, 255, 0, 255);
		targetColour = new Color32 (255, 255, 0, 255);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (GameGrid.selectedNode != null) {
			if (this.spriteRen.color == moveableColour && GameGrid.selectedNode.unit.GetComponent<Ship> ().moving == true) {
				GameGrid.selectedNode.ResetNeighbours ();
				GameGrid.selectedNode.unit.transform.position = this.transform.position;
				unit = GameGrid.selectedNode.unit;
				GameGrid.selectedNode.unit = null;
				GameGrid.selectedNode.traversable = true;
				traversable = false;
				unit.GetComponent<Ship> ().moving = false;
				unit.GetComponent<Ship> ().shooting = true;
			}
			else if(this.spriteRen.color == targetColour && GameGrid.selectedNode.unit.GetComponent<Ship> ().shooting == true)
			{
				int damage = GameGrid.selectedNode.unit.GetComponent<ShipShooting> ().DamageTarget();
				unit.GetComponent<ShipHealth> ().TakeDamage (damage);
				GameGrid.selectedNode.ResetNeighbours ();
				GameGrid.selectedNode.unit.GetComponent<Ship>().shooting = false;
			}
			else
			{
				GameGrid.selectedNode.ResetNeighbours ();
			}
		}
		GameGrid.selectedNode = this;
		CurrentAction ();
	}

	public void CurrentAction(){
		if (unit != null && unit.GetComponent<Ship>().shooting && unit.GetComponent<ShipShooting> () == true) {
			Node[] neighbours = unit.GetComponent<ShipShooting>().ShotRange(gridPosition);
			SetSquareType (false, neighbours);
			unit.GetComponent<Ship>().activated = true;
		}

		else if (unit != null && unit.GetComponent<ShipMovement> () == true && !unit.GetComponent<Ship>().activated && !unit.GetComponent<Ship>().enemy) {
			Node[] neighbours = unit.GetComponent<ShipMovement>().MoveRange(gridPosition);
			SetSquareType (true, neighbours);
			unit.GetComponent<Ship> ().moving = true;
		}
    }

	public void SetSquareType(bool isMove,Node[] neighbours){
		if (neighbours.Length != 0) {
			for (int i = 0; i < neighbours.Length; i++) {
				if (neighbours [i].traversable && isMove) {
					neighbours [i].SetColour (moveableColour);
				}
				else if (!neighbours [i].traversable && !isMove && neighbours [i].unit != null && neighbours [i].unit.GetComponent<ShipHealth> () != false) {
					if (neighbours [i].gridPosition != gridPosition) {
						neighbours [i].SetColour (targetColour);
					}
				}
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
				neighbours = unit.GetComponent<ShipShooting> ().ShotRange (gridPosition);
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
