using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
        transform.position = GetComponentInParent<Transform>().position;
	}
	
	// Update is called once per frame
	void update () {
        if (target != null)
        {
            Debug.Log("ilive");
            //while (transform.position != target.transform.position)
            //{
                //transform.Translate(Vector3.MoveTowards(transform.position, target.transform.position, .1f));
            //}
            Destroy(this);
        }
	}
}
