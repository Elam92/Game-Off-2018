using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
	float healthShrink;
	// Use this for initialization
	void Start () {
		if (gameObject.GetComponentInParent<ShipHealth> () == true) {
			healthShrink = transform.localScale.y / gameObject.GetComponentInParent<Ship>().GetHealth();
			GetComponent<MeshRenderer> ().enabled = true;
			transform.localPosition = new Vector3 (0f, -.5f, 0f);
		}
	}
	
	// Update is called once per frame
	public void ReduceBar (int damage) {
		for (int i = 0; i < damage; i++) {
			transform.localScale -= new Vector3 (0f, healthShrink, 0f);
		}
	}
}
