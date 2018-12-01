using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

[RequireComponent(typeof(GameGridGenerator))]
public class GameGrid : MonoBehaviour {
    
    public LayerMask untraversableMask;

    public static Node selectedNode;

	private static Node[,] grid;

    public static  bool playerTurn = true;

    private static GameObject[] curShips;
    private static GameObject[] aiShips;

    private static int actedShips = 0;

    private void Awake()
    {
        CalculateGrid();
		Button endTurnButton = Button.FindObjectOfType<Button> ();
		endTurnButton.onClick.AddListener (ForceEnd);
        SetTurn();
    }

	public static void SetUI(){
		GameObject current = GameObject.Find ("HealthValue");
		current.GetComponent<Text> ().text = selectedNode.unit.GetComponent<ShipHealth> ().health.ToString();
		current = GameObject.Find ("SpeedValue");
		current.GetComponent<Text> ().text = selectedNode.unit.GetComponent<ShipMovement> ().MovementSpeed.ToString();
		current = GameObject.Find ("RangeValue");
		current.GetComponent<Text> ().text = selectedNode.unit.GetComponent<ShipShooting> ().weaponRange.ToString();
		current = GameObject.Find ("DamageValue");
		current.GetComponent<Text> ().text = selectedNode.unit.GetComponent<ShipShooting> ().weaponDamage.ToString();
	}

    public void CalculateGrid()
    {
        Transform generatedGrid = transform.GetChild(0);
        GameGridGenerator generator = GetComponent<GameGridGenerator>();

		int rowSize = (int)generator.gridWorldSize.x;
		int colSize = (int)generator.gridWorldSize.y;

        grid = new Node[rowSize, colSize];

        if (generatedGrid != null && (rowSize * colSize == generatedGrid.childCount))
        {
            int index = 0;

            for (int y = 0; y < colSize; y++)
            {
                for (int x = 0; x < rowSize; x++)
                {
                    Node node = generatedGrid.GetChild(index).GetComponent<Node>();
                    grid[x, y] = node;
					node.gridPosition = new int[]{x, y};
                    index++;
                }
            }
        }
    }

    // FROM HERE
    public static void SetTurn()
    {
        if (!playerTurn)
        {
            aiShips = GetShips("AiShip");
            actedShips = 0;

            for (int i = 0; i < aiShips.Length; i++)
            {
                aiShips[i].GetComponent<AIShip>().DoActions();
            }
            //run Ai script and keep track of ships with movedShip(){}
        }
		else if(playerTurn)
        {
            curShips = GetShips("PlayerShip");
            actedShips = 0;
            //let player act
        }
    }

    public static GameObject[] GetShips(string turn) 
    {
        return GameObject.FindGameObjectsWithTag(turn);
    }

    public static void MovedShip() 
    {
        actedShips += 1;
        if (playerTurn == true)
        {
            if (actedShips >= curShips.Length)
            {
                foreach (GameObject ship in curShips)
                {
                    ship.GetComponent<Ship>().activated = false;
                    ship.GetComponent<Ship>().moving = false;
                    ship.GetComponent<Ship>().shooting = false;
                }
                EndTurn();
            }
        }
        else
        {
            if(actedShips >= aiShips.Length)
            {
                EndTurn();
            }
        }
    }

    public static void EndTurn() 
    {
        if (!playerTurn)
        {
            playerTurn = true;
            SetTurn();
        }
		else if(playerTurn)
        {
            playerTurn = false;
            SetTurn();
        }
    }

	void ForceEnd()
	{
		foreach (GameObject ship in curShips)
		{
			ship.GetComponent<Ship>().activated = false;
			ship.GetComponent<Ship>().moving = false;
			ship.GetComponent<Ship>().shooting = false;
		}
		EndTurn();
	}
    //TO HERE MIGHT BE OWN SCRIPT
	public static List<Node> GetNeighbours(int[] gridPosition, int range,List<Node> nodeList,int current,bool isShoot)
    {
        int row = gridPosition[0];
        int col = gridPosition[1];
		int[] next = new int[]{row, col};
		int[] directions = new int[]{-1,0,1,0,0,-1,0,1};
		if (current >= range)
		{
			return nodeList;
		}

		if (!nodeList.Contains(grid [row, col])) 
		{
			nodeList.Add (grid [row, col]);
		}

		// using the array of directional ints works through all 4 dirrections for the current node and when there is a possible move it recursivly calls GetNeighbour again
		for (int i = 0; i < directions.Length; i += 2) {
			if (row + directions [i] >= 0 && row + directions [i] <= grid.GetLength (0) - 1) {
				if (col + directions [i + 1] >= 0 && col + directions [i + 1] <= grid.GetLength (1) - 1) {
					if(!nodeList.Contains(grid[row + directions[i], col + directions[i+1]]))
					{
						nodeList.Add(grid[row + directions[i], col + directions[i+1]]);
					}
					next [0] += directions [i];
					next [1] += directions [i + 1];
					if (grid [next [0], next [1]].traversable == true || isShoot) {
						nodeList = GetNeighbours (next, range, nodeList, current + 1,isShoot);
					}
					next [0] -= directions [i];
					next [1] -= directions [i + 1];
				}
			}
		}
        return nodeList;
    }

    public static List<Node> GetNearestNeighbours(int[] gridPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = gridPosition[0] + x;
                int checkY = gridPosition[1] + y;

                if(checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public static List<Node> FindPath(Node from, Node to)
    {
        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(from);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost ||
                   openSet[i].fCost == currentNode.fCost && openSet[i].gCost < currentNode.gCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode.Equals(to))
            {
                return RetracePath(from, to);
            }

            foreach (Node neighbour in GetNearestNeighbours(currentNode.gridPosition))
            {
                if (neighbour.traversable == false && neighbour.unit == null || 
                    closedSet.Contains(neighbour) || 
                    neighbour.unit != null && neighbour.unit.tag.Equals("AiShip"))
                {
                    continue;
                }

                int movementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, to);
                    neighbour.parent = currentNode; 

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        return path;
    }

    private static int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridPosition[0] - b.gridPosition[0]);
        int dstY = Mathf.Abs(a.gridPosition[1] - b.gridPosition[1]);

        /*
        if(dstX > dstY)
        {
            return 14*dstY + 10*(dstX - dstY);
        }

        return 14*dstX + 10*(dstY - dstX); */

        return (dstX <= dstY) ? dstX : dstY;
    }
}
