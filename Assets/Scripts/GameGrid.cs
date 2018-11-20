using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(GameGridGenerator))]
public class GameGrid : MonoBehaviour {
    
    public LayerMask untraversableMask;

    public static Node selectedNode;

	private static Node[,] grid;

    private void Awake()
    {
        CalculateGrid();
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

    // TODO: Use Range parameter.
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

		//deals with first node
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
					nodeList = GetNeighbours(next, range, nodeList, current + 1);
					next [0] -= directions [i];
					next [1] -= directions [i + 1];
				}
			}
		}
        return nodeList;
    }


}
