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
	public Node[] ShotRange(int[] gridPosition, Transform unit){
		List<Node> neighbours = GameGrid.GetNeighbours (gridPosition, weaponRange, new List<Node> (), 0);
		for (int i = neighbours.Count - 1; i >= 0; i--) {
			if (neighbours [i].traversable ||neighbours [i].unit == null || neighbours [i].unit.GetComponent<ShipHealth> () == false || unit.tag == neighbours [i].unit.tag) {
				neighbours.RemoveAt(i);
			}
		}
		return neighbours.ToArray ();
	}

	public Node[] ResetRange(int[] gridPosition){
		List<Node> neighbours = GameGrid.GetNeighbours (gridPosition, weaponRange, new List<Node> (), 0);
		return neighbours.ToArray ();
	}

	public int DamageTarget(){
		return weaponDamage;
	}
}
