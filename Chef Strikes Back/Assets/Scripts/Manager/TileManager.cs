using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    [SerializeField] Grid grid;
    private List<Vector2> entrance;
    private Dictionary<Vector2, bool> chairs;
    private List<Vector2> emptySpot;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Start()
    {
        entrance = new List<Vector2>();
        foreach (var tilemap in grid.GetComponentsInChildren<Tilemap>())
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                var tile = tilemap.GetTile<EntranceTile>(position);
                if (tile == null) continue;
                entrance.Add(tilemap.CellToWorld(position)); ;
            }
        }

        chairs = new Dictionary<Vector2, bool>();
        foreach (var tilemap in grid.GetComponentsInChildren<Tilemap>())
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                var tile = tilemap.GetTile<ChairTile>(position);
                if (tile == null) continue;
                chairs.TryAdd(tilemap.CellToWorld(position), false); ;
            }
        }


        emptySpot = new List<Vector2>();
        foreach (var tilemap in grid.GetComponentsInChildren<Tilemap>())
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                var tile = tilemap.GetTile<EntranceTile>(position);
                if (tile == null) continue;
                emptySpot.Add(tilemap.CellToWorld(position)); ;
            }
        }
    }

    public Vector2 requestEntrancePos()
    {
        foreach (var item in entrance)
        {
            if (item == null) continue;
            return item;
        }

        return new Vector2(int.MaxValue, int.MaxValue);
    }

    public Vector2 requestEmptyPos()
    {
        int random = UnityEngine.Random.Range(0, checkWalkableCount());
        return emptySpot[random];
    }

    public Vector2 requestChairPos()
    {
        foreach (var item in chairs)
        {
            if (!item.Value)
            {
                chairs[item.Key] = true;
                return item.Key;
            }
        }

        return new Vector2(int.MaxValue, int.MaxValue);
    }
    public bool checkChairCount()
    {
        int count = 0;
        foreach (var item in chairs)
        {
            if (!item.Value) count++;
        }

        if (count > 0) return true;
        return false;
    }

    public int checkWalkableCount()
    {
        int count = 0;
        foreach (var item in emptySpot)
        {
            count++;
        }
        return count;
    }
}
