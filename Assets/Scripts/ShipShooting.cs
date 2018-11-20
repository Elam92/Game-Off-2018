using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour {
	public int weaponRange;
	public int weaponDamage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public Node[] ShotRange(int[] gridPosition){
		List<Node> neighbours = GameGrid.GetNeighbours (gridPosition, weaponRange, new List<Node> (), 0);
		return neighbours.ToArray ();
	}

	public int DamageTarget(){
		return weaponDamage;
	}
}
