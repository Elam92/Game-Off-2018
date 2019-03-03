using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int shield;

    public event EventHandler OnDeath;

    private int maxHealth;
    // Use this for initialization
    void Start()
    {
        maxHealth = health;
    }

    // CHECK IF MEMORY LEAKS
    private void OnDestroy()
    {
    //    Debug.Log(gameObject.name + " IS LOST!");
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath.Invoke(this, EventArgs.Empty);
        }
    }

    /*
    public bool TakeDamage(int damage)
    {
        if (health == maxHealth)
        {
            GameObject.Instantiate(GameObject.Find("HealthBar"), gameObject.transform);
        }
        GetComponentInChildren<HealthBar>().ReduceBar(damage);
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);

            return true;
        }
        return false;
    }
    */
}
