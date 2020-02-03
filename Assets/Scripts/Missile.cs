using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEventArgs
{
    public Ship target;

    public OnHitEventArgs(Ship newTarget)
    {
        target = newTarget;
    }
}

public class Missile : MonoBehaviour {
    public Transform target;

    public event EventHandler<OnHitEventArgs> OnHit;

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
			transform.position = Vector3.MoveTowards(transform.position, target.position, 4 * Time.deltaTime);
			if (transform.position == target.transform.position) {
                OnHit(this, new OnHitEventArgs(target.GetComponent<Ship>()));
				Destroy (gameObject);
			}
        }
	}
}
