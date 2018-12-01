using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(GameGridGenerator))]
public class GameGrid : MonoBehaviour {
    
    public LayerMask untraversableMask;

    public static Node selectedNode;

	private static Node[,] grid;

    public static  bool playerTurn = true;

    private static GameObject[] curShips;

    private static int actedShips = 0;

    private void Awake()
    {
        CalculateGrid();
        SetTurn();
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
        Debug.Log("set turn");
        if (playerTurn == false)
        {
            Debug.Log("AI turn");
            GetShips("AiShip");
            actedShips = 0;
            //run Ai script and keep track of ships with movedShip(){}
        }
        else
        {
            Debug.Log("player turn");
            GetShips("PlayerShip");
            actedShips = 0;
            //let player act
        }
    }

    public static void GetShips(string turn) 
    {
        curShips = GameObject.FindGameObjectsWithTag(turn);
        Debug.Log("curShips = " + curShips.Length.ToString());
    }

    public static void MovedShip() 
    {
        actedShips += 1;
        if (actedShips == curShips.Length)
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

    public static void EndTurn() 
    {
        if (playerTurn == false)
        {
            playerTurn = true;
            SetTurn();
        }
        else
        {
            playerTurn = false;
            SetTurn();
        }
    }

    //TO HERE MIGHT BE OWN SCRIPT
	public static List<Node> GetNeighbours(int[] gridPosition, int range,List<Node> nodeList,int current)
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
					if (grid [next [0], next [1]].traversable == true) {
						nodeList = GetNeighbours (next, range, nodeList, current + 1);
					}
					next [0] -= directions [i];
					next [1] -= directions [i + 1];
				}
			}
		}
        return nodeList;
    }


}
