using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(GameGrid))]
public class GameGridGenerator : MonoBehaviour
{

    public Transform gridNodePrefab;
    public LayerMask untraversableMask;
    public Vector2 gridWorldSize;

    public bool generateGrid = false;

    private void Update()
    {
        if (generateGrid)
        {
            GenerateGrid();
            generateGrid = false;
        }
    }

    public void GenerateGrid()
    {
        Debug.Log("GOO");

        Transform oldGrid = transform.Find("GeneratedGrid");

        if (oldGrid != null)
        {
            Debug.Log("Destroying old Grid");
            DestroyImmediate(oldGrid.gameObject);
        }

        Transform grid = new GameObject().transform;
        grid.name = "GeneratedGrid";
        grid.position = transform.position;

        int width = Screen.width;
        int height = Screen.height;

        Vector2 nodeSize = new Vector2(gridNodePrefab.localScale.x, gridNodePrefab.localScale.y);

        Vector2 nodeOffset = new Vector2(gridNodePrefab.localScale.x / 10f, gridNodePrefab.localScale.y / 10f);

        int rowSize = Mathf.RoundToInt(gridWorldSize.x / 2);
        int colSize = Mathf.RoundToInt(gridWorldSize.y / 2);

        float offSetX = transform.position.x - (Vector3.right.x * rowSize) - nodeOffset.x;
        float offsetY = transform.position.y + (Vector3.up.y * colSize) + nodeSize.y + (nodeOffset.y * colSize);

        Vector3 nextPosition = new Vector3(offSetX, offsetY, 0f);

        for (int y = 0; y < gridWorldSize.y; y++)
        {
            for (int x = 0; x < gridWorldSize.x; x++)
            {
                Transform gridNode = Instantiate(gridNodePrefab, nextPosition, Quaternion.identity);

                nextPosition.x += nodeSize.x + nodeOffset.x;

                gridNode.SetParent(grid);
                gridNode.name = "Grid (" + x + ", " + y + ")";

                Node nodeScript = gridNode.GetComponent<Node>();
                nodeScript.traversable = true;
            }
            nextPosition.x = offSetX;
            nextPosition.y -= nodeSize.y + nodeOffset.y;

        }

        grid.SetParent(transform);



    }


}
