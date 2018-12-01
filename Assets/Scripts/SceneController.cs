using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public Vector3 target;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, target, 1 * Time.deltaTime);
		if (transform.position == target) {
			int index = SceneManager.GetActiveScene().buildIndex + 1;
			if (index >= SceneManager.sceneCountInBuildSettings)
			{
				index = 0;
			}

			SceneManager.LoadScene (index);
		}
	}
}
