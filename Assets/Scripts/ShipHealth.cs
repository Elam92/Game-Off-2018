using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour {
	public int health;
	public int shield;
	private int maxHealth;
	// Use this for initialization
	void Start () {
		maxHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(int damage){
		if (health == maxHealth) {
			GameObject.Instantiate (GameObject.Find ("HealthBar"), gameObject.transform);
		}
		health -= damage;
		if (health <= 0){
			gameObject.SetActive(false);
		}
	}
}
