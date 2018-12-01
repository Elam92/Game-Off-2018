using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : MonoBehaviour {

    public bool moving = false;
    public bool shooting = false;
    public bool activated = false;

    public Ship target;
    public Node currentNode;

    private ShipMovement shipMovement;
    private ShipHealth shipHealth;
    private ShipShooting shipShooting;

    void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipHealth = GetComponent<ShipHealth>();
        shipShooting = GetComponent<ShipShooting>();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            FindPath();
        }
    }

    private void FindPath()
    {
        List<Node> path = GameGrid.FindPath(currentNode, target.currentNode);

        for (int i = 0; i < path.Count; i++)
        {
            Color32 rndColour = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            path[i].SetColour(rndColour);
        }
    }
}
