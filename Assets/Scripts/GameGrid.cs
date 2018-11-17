using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {
    
    public LayerMask untraversableMask;
    private Node[,] grid;

    private void Awake()
    {
        
    }

    public void GenerateGrid()
    {
        Transform grid = transform.GetChild(0);


    }


}
