using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
   
    public bool displayGridGizmos;
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();
    [SerializeField]Transform AI;
    Tilemap tilemap;

    Node[,] grid;

    float nodeDiameter;
    Vector2Int offset;
    Vector2Int gridSize;

    
  

    public int MaxSize
    {
        get
        {
            return gridSize.x * gridSize.y;
        }
    }

    

    public void CreateNodeList(List<Vector3Int> position, Tilemap tilemap)
    {
        Vector2Int max = new(tilemap.cellBounds.xMax, tilemap.cellBounds.yMax);
        offset.x = tilemap.cellBounds.xMin;
        offset.y = tilemap.cellBounds.yMin;
        gridSize.x = max.x - offset.x;
        gridSize.y = max.y - offset.y;

        var cellSizeY = (tilemap.layoutGrid.cellSize.y );
        var cellSizeZ = (tilemap.layoutGrid.cellSize.z);

        Vector3 cellSize = new Vector3(0, cellSizeY, cellSizeZ);

        grid = new Node[gridSize.x, gridSize.y];
        foreach(var pos in position)
        {
            grid[pos.x - tilemap.cellBounds.xMin, pos.y - tilemap.cellBounds.yMin] = new Node(true, tilemap.CellToWorld(pos) , pos.x - offset.x, pos.y - offset.y, 0);
        }

    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    var Node = grid[checkX, checkY];
                    if (Node != null)
                        neighbours.Add(Node);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector2 worldPosition, Tilemap tilemap)
    {
        Vector3Int pos = tilemap.WorldToCell(worldPosition);


        if (pos.x - offset.x < 0 || pos.x - offset.x > grid.GetUpperBound(0) || pos.y - offset.y < 0 || pos.y - offset.y > grid.GetUpperBound(1))
        {
            Debug.LogError((pos.x - offset.x) + " - " + (pos.y - offset.y));
            return null;
        }

        return grid[pos.x - offset.x, pos.y - offset.y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            
            foreach (Node n in grid)
            {
                if (n == null) continue;

                
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(n.gridX == 0 && n.gridY == 6) Gizmos.color = Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - .1f));

            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
