using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
	public int MovementSpeed;
	private Node curTarget;
	// Use this for initialization
	void Start () {
	}

	void Update (){
	}
		
	public Node[] MoveRange(int[] gridPosition){
		List<Node> neighbours = GameGrid.GetNeighbours (gridPosition, MovementSpeed, new List<Node> (), 0);
		return neighbours.ToArray ();
	}

	public void MoveSprite(Node current, Node target){
		StartCoroutine (MakeMove(current,target));
		target.unit = GameGrid.selectedNode.unit;
		current.unit = null;
		current.traversable = true; 
		target.traversable = false;
        GetComponent<Ship>().currentNode = target;
		GetComponent<Ship>().moving = false;
		GetComponent<Ship>().shooting = true;
	}

	IEnumerator MakeMove(Node current, Node target){
		Vector3 lookAt;
		Vector3 curPosition;
		List<Node> path = GameGrid.FindPath (current, target);
		for (int i = 0; i < path.Count; i++) {
			lookAt = path [i].transform.position - transform.position;
			curPosition = transform.position;
			curPosition.x = 0;
			transform.rotation = Quaternion.FromToRotation (curPosition, lookAt);
			transform.position = path[i].transform.position;
			yield return new WaitForSeconds(0.2f);
		}
	}
}
