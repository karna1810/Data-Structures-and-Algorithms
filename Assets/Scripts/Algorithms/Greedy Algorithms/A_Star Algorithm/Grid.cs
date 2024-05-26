using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;

    [Header("Grid Parameters")]
    [SerializeField] private Vector2 gridWorldSize;

    [Header("Node")]
    [SerializeField] private float nodeRadius;
    private Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX,gridSizeY;    

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX=Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY=Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid=new Node[gridSizeX,gridSizeY];

        Vector3 gridBottomLeft = this.transform.position + (Vector3.left*gridSizeX/2) + (Vector3.back*gridSizeY/2);

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0;y< gridSizeY; y++)
            {
                Vector3 _worldPosition = gridBottomLeft+Vector3.right*(x*nodeDiameter+nodeRadius)+Vector3.forward*(y*nodeDiameter+nodeRadius);
                bool _isWalkable = !(Physics.CheckSphere(_worldPosition, nodeRadius, obstacleMask));

                grid[x, y] = new Node(_isWalkable, _worldPosition);
            }
        }
    }

    public void OnDrawGizmos()
    {
        //this draws the bounds of the grid
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        //this draws the individual nodes of the grid
        if(grid!=null)
        {
            foreach(Node node in grid)
            {
                Gizmos.color=node.isWalkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }

    }

    //the class for a individual node(or) cell in the grid 
    public class Node
    {
        public bool isWalkable;
        public Vector3 worldPosition;

        //the constructor for the class
        public Node(bool _isWalkable, Vector3 _worldPosition)
        {
            this.isWalkable = _isWalkable;
            this.worldPosition = _worldPosition;
        }
    }
}
