using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameGridGenerator))]
public class GameGrid : MonoBehaviour
{
    public enum NodeStates
    {
        Normal,
        Selectable,
        Moveable,
        WeaponRange,
        Targetable
    }
    public LayerMask untraversableMask;

    public static bool playerTurn = true;

    private static Node[,] grid;
    private static Dictionary<NodeStates, Color32> nodeStateColours;

    public Button endTurnButton;
    public Button endGameButton;

    private void Awake()
    {
        Color32 originalColour = new Color32(255, 255, 255, 30);
        Color32 selectableColour = new Color32(241, 222, 28, 143);
        Color32 moveableColour = new Color32(0, 255, 0, 40);
        Color32 targetColour = new Color32(255, 0, 0, 60);
        Color32 weaponRangeColour = new Color32(0, 0, 255, 60);

        nodeStateColours = new Dictionary<NodeStates, Color32>
        {
            { NodeStates.Normal, originalColour },
            { NodeStates.Selectable, selectableColour },
            { NodeStates.Moveable, moveableColour },
            { NodeStates.Targetable, targetColour },
            { NodeStates.WeaponRange, weaponRangeColour }
        };


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
                    node.gridPosition = new int[] { x, y };
                    index++;
                }
            }
        }
    }

    public static Node GetNode(int[] gridPosition)
    {
        return grid[gridPosition[0], gridPosition[1]];
    }

    public static void UpdateNodeState(Node node, NodeStates state, Action<Node> updateNode = null)
    {
        node.SetColour(nodeStateColours[state]);
        updateNode?.Invoke(node);
    }

    public static void UpdateNodeStates(Node[] nodes, NodeStates state, Action<Node> updateNode = null)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetColour(nodeStateColours[state]);
            updateNode?.Invoke(nodes[i]);
        }
    }

    // TO HERE MIGHT BE OWN SCRIPT
    // Used for Shooting and Moving.
    public static List<Node> GetNeighbours(int[] gridPosition, int range, List<Node> nodeList, int current, bool isShoot)
    {
        int row = gridPosition[0];
        int col = gridPosition[1];
        int[] next = { row, col };
        int[] directions = { -1, 0, 1, 0, 0, -1, 0, 1 };
        if (current >= range)
        {
            return nodeList;
        }

        if (!nodeList.Contains(grid[row, col]))
        {
            nodeList.Add(grid[row, col]);
        }

        // using the array of directional ints works through all 4 dirrections for the current node and when there is a possible move it recursivly calls GetNeighbour again
        for (int i = 0; i < directions.Length; i += 2)
        {
            if (row + directions[i] >= 0 && row + directions[i] < grid.GetLength(0))
            {
                if (col + directions[i + 1] >= 0 && col + directions[i + 1] < grid.GetLength(1))
                {
                    if (!nodeList.Contains(grid[row + directions[i], col + directions[i + 1]]))
                    {
                        nodeList.Add(grid[row + directions[i], col + directions[i + 1]]);
                    }
                    next[0] += directions[i];
                    next[1] += directions[i + 1];
                    if (grid[next[0], next[1]].traversable == true || isShoot)
                    {
                        nodeList = GetNeighbours(next, range, nodeList, current + 1, isShoot);
                    }
                    next[0] -= directions[i];
                    next[1] -= directions[i + 1];
                }
            }
        }
        return nodeList;
    }

    // Used in Finding a Path.
    public static List<Node> GetNearestNeighbours(int[] gridPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = gridPosition[0] + x;
                int checkY = gridPosition[1] + y;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    // Find the path for the object to move through.
    public static List<Node> FindPath(Node from, Node to)
    {
        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(from);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                   openSet[i].FCost == currentNode.FCost && openSet[i].gCost < currentNode.gCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(to))
            {
                return RetracePath(from, to);
            }

            foreach (Node neighbour in GetNearestNeighbours(currentNode.gridPosition))
            {
                // Conditions: Obstacles, already visited nodes, and ship-occupied nodes that isn't the target
                if (neighbour.traversable == false && neighbour.unit == null ||
                    closedSet.Contains(neighbour) ||
                    neighbour.unit != null && neighbour != to)
                {
                    continue;
                }

                int movementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, to);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
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
