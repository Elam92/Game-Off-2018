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

	public void fireMissle (Node node){
		Vector3 lookAt;
		Vector3 curPosition;
		lookAt = node.unit.position - transform.position;
		curPosition = transform.position;
		curPosition.x = 0;
		transform.rotation = Quaternion.FromToRotation (curPosition, lookAt);
		GameObject.Instantiate(GameObject.Find("Missle"), transform).GetComponent<Missle>().target = node.unit;
		bool dead = node.unit.GetComponent<ShipHealth>().TakeDamage(weaponDamage);
		if (dead) {
			node.traversable = true;
			node.unit = null;
		}
		GetComponent<Ship>().shooting = false;
		GetComponent<Ship>().activated = true;
		GameGrid.MovedShip();
	}
}
