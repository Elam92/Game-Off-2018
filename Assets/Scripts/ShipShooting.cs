using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
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

    public int GetWeaponRange()
    {
        return weaponRange;
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public Node[] ShowWeaponRange(int[] gridPosition)
    {
        List<Node> neighbours = GameGrid.GetNeighbours(gridPosition, weaponRange, new List<Node>(), 0, true);
        for (int i = neighbours.Count - 1; i >= 0; i--)
        {
            if (neighbours[i].traversable || neighbours[i].unit == null || neighbours[i].unit.GetComponent<ShipHealth>() == false || tag.Equals(neighbours[i].unit.tag))
            {
                neighbours.RemoveAt(i);
            }
        }
        return neighbours.ToArray();
    }

    public Node[] ResetRange(int[] gridPosition)
    {
        List<Node> neighbours = GameGrid.GetNeighbours(gridPosition, weaponRange, new List<Node>(), 0, true);
        return neighbours.ToArray();
    }

    public int DamageTarget()
    {
        return weaponDamage;
    }

    public void Fire(Node node)
    {
        Vector3 lookAt;
        Vector3 curPosition;
        lookAt = node.unit.position - transform.position;
        curPosition = transform.position;
        curPosition.x = 0;
        transform.rotation = Quaternion.FromToRotation(curPosition, lookAt);
        Instantiate(GameObject.Find("Missle"), transform).GetComponent<Missle>().target = node.unit;

        if (audioSource != null)
        {
            if (fireSFX != null)
            {
                audioSource.PlayOneShot(fireSFX);
            }
        }

        isFiring = true;
    }
}
