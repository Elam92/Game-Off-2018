using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
			if (!gameObject.GetComponent<MeshRenderer> ().enabled) {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			}
			transform.position = Vector3.MoveTowards(transform.position, target.position, 1 * Time.deltaTime);
			if (transform.position == target.transform.position) {
				Destroy (gameObject);
			}
        }
	}
}
