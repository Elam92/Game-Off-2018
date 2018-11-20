using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
	public int MovementSpeed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public Node[] MoveRange(int[] gridPosition){
		List<Node> neighbours = GameGrid.GetNeighbours (gridPosition, MovementSpeed, new List<Node> (), 0);
		return neighbours.ToArray ();
	}
}
