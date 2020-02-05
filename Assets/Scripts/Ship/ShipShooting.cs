using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public Missile projectile;
    public AudioClip fireSFX;

    [SerializeField]
    private int weaponRange = 1;
    [SerializeField]
    private int weaponDamage = 1;

    private bool isFiring = false;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public int GetWeaponDamage()
    {
        return weaponDamage;
    }

    public int GetWeaponRange()
    {
        return weaponRange;
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public (Node[] range, Node[] targets)? ShowWeaponRange(int[] gridPosition)
    {
        List<Node> range = GameGrid.GetNeighbours(gridPosition, weaponRange, new List<Node>(), 0, true);
        List<Node> targets = new List<Node>();
        for (int i = range.Count - 1; i >= 0; i--)
        {
            // If node is occupied and is an Enemy ship.
            if(range[i].unit != null && range[i].unit.GetComponent<Ship>() == true && !tag.Equals(range[i].unit.tag))
            {
                targets.Add(range[i]);
                range.RemoveAt(i);
            }
            // If node is not a ship and is not traversable.
            else if(!range[i].traversable)
            {
                range.RemoveAt(i);
            }
        }
        return (range.ToArray(), targets.ToArray());
    }

    public void Fire(Node node)
    {
        StartCoroutine(FireAction(node));
    }

    IEnumerator FireAction(Node node)
    {
        isFiring = true;

        Vector3 direction;
        direction = node.unit.position - transform.position;
        transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        Missile missile = Instantiate(projectile, transform);
        missile.target = node.unit;
        missile.OnHit += DamageShip;

        if (audioSource != null)
        {
            if (fireSFX != null)
            {
                audioSource.PlayOneShot(fireSFX);
            }
        }

        Debug.Log("TARGET SHIP: " + node.unit.name);
        while(isFiring)
        {
            yield return null;
        }
    }

    private void DamageShip(object sender, OnHitEventArgs e)
    {
        if (e.target != null)
        {
            e.target.TakeDamage(weaponDamage);
        }

        Missile missile = (Missile)sender;
        missile.OnHit -= DamageShip;

        isFiring = false;
    }
}
