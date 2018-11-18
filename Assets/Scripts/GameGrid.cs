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

        grid = new Node[colSize, rowSize];

        if (generatedGrid != null && (rowSize * colSize == generatedGrid.childCount))
        {
            int index = 0;

            for (int y = 0; y < colSize; y++)
            {
                for (int x = 0; x < rowSize; x++)
                {
                    Node node = generatedGrid.GetChild(index).GetComponent<Node>();
                    grid[y, x] = node;
                    node.gridPosition = new int[]{y, x};
                    index++;
                }
            }
        }
    }

    // TODO: Use Range parameter.
    public static Node[] GetNeighbours(int[] gridPosition, int range)
    {
        int row = gridPosition[0];
        int col = gridPosition[1];

        List<Node> nodeList = new List<Node>();

        // Left Neighbour.
        if(row > 0)
        {
            nodeList.Add(grid[row - 1, col]);
        }

        // Right Neighbour.
        if(row < grid.GetLength(0) - 1)
        {
            nodeList.Add(grid[row + 1, col]);
        }

        // Top Neighbour.
        if(col > 0)
        {
            nodeList.Add(grid[row, col - 1]);
        }

        // Bottom Neighbour.
        if(col < grid.GetLength(1) - 1)
        {
            nodeList.Add(grid[row, col + 1]);
        }

        return nodeList.ToArray();
    }


}
