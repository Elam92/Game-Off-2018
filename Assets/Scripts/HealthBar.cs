using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (gameObject.GetComponentInParent<ShipHealth> () == true) {
			GetComponent<MeshRenderer> ().enabled = true;
//			Transform holder =  transform.localPosition;
//			holder.localPosition.y -= .5f;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
