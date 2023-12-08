using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }


    public List<Tile> GetPathRunable(Tile origin, Tile destination)
    {
        gridManager.ClearTilePathfindingData();

        List<Tile> path = new List<Tile>();
        Queue<Tile> open = new Queue<Tile>();
        List<Tile> closed = new List<Tile>();
        List<Tile> neighbours = new List<Tile>();

        Tile current = origin;

        open.Enqueue(current);

        int iter = 0;

        while(open.Count != 0 && !closed.Contains(destination))
        {
            iter++;
            if(iter > 5000)
            {
                Debug.LogError("GetPathRunable не удалось найти путь");
                for (int i = 0; i < closed.Count; i++)
                {
                    CreateText(closed[i].transform.position, closed[i].tilePathfindingData.total);
                }

                return null;
            }
            current = open.Dequeue();

            neighbours = gridManager.GetNeighboursCross(current);

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (!closed.Contains(neighbours[i]) && !open.Contains(neighbours[i]) && !neighbours[i].isObstacle)
                {
                    neighbours[i].tilePathfindingData.parent = current;
                    neighbours[i].tilePathfindingData.distance = gridManager.Distance(neighbours[i], destination);
                    neighbours[i].tilePathfindingData.moveCount = gridManager.Distance(neighbours[i], origin);

                    open.Enqueue(neighbours[i]);
                }
            }

            closed.Add(current);
        }

        current = destination;

        while (current != origin)
        {
            path.Add(current);
            current = current.tilePathfindingData.parent;
        }

        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("Path " + i + " = " + path[i].index.ToString());
        }

        path.Reverse();
        return path;
    }



    public bool debugText = false;
    private void CreateText(Vector3 position, float total)
    {
        var go = new GameObject();
        var text = go.AddComponent<TextMesh>();
        go.transform.position = position;

        text.text = total.ToString();

        go.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        go.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
        text.color = Color.black;
    }

    public List<Tile> GetPathFlyable(Tile origin, Tile destination)
    {
        gridManager.ClearTilePathfindingData();

        List<Tile> path = new List<Tile>();
        Queue<Tile> open = new Queue<Tile>();
        List<Tile> closed = new List<Tile>();
        List<Tile> neighbours = new List<Tile>();

        Tile current = origin;

        open.Enqueue(current);

        int iter = 0;

        while (open.Count != 0 && !closed.Contains(destination))
        {
            iter++;
            if (iter > 5000)
            {
                Debug.LogError("GetPathRunable не удалось найти путь");
                for (int i = 0; i < closed.Count; i++)
                {
                    CreateText(closed[i].transform.position, closed[i].tilePathfindingData.total);
                }

                return null;
            }
            current = open.Dequeue();

            neighbours = gridManager.GetNeighboursDiagonal(current);

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (!closed.Contains(neighbours[i]) && !open.Contains(neighbours[i]))
                {
                    neighbours[i].tilePathfindingData.parent = current;
                    neighbours[i].tilePathfindingData.distance = gridManager.Distance(neighbours[i], destination);
                    neighbours[i].tilePathfindingData.moveCount = gridManager.Distance(neighbours[i], origin);

                    open.Enqueue(neighbours[i]);
                }
            }

            closed.Add(current);
        }

        current = destination;

        while (current != origin)
        {
            path.Add(current);
            current = current.tilePathfindingData.parent;
        }

        path.Reverse();
        return path;
    }

    public List<Tile> safeTiles = new List<Tile>();
    public List<Tile> GetPathToSafeTile(List<Tile> deadZone, Tile position,Tile characterPosition,int range, bool canFly)
    {
        Dictionary<Tile, float> distance = new Dictionary<Tile, float>();

        safeTiles = gridManager.GetRange(characterPosition, range + 1);
        for (int i = 0; i < safeTiles.Count; i++)
        {
            if (!deadZone.Contains(safeTiles[i]))
            {
                Debug.Log("KEK " + safeTiles[i].index.ToString());
            }
        }
        safeTiles.RemoveAll(x => deadZone.Contains(x) || x.isObstacle);

        Debug.Log("Safe Tile Count: " + safeTiles.Count);
        for (int i = 0;i < safeTiles.Count;i++) 
        {
            distance.Add(safeTiles[i], gridManager.Distance(position, safeTiles[i]));
        }

        float minVal = Mathf.Infinity;
        Tile min = null;
        foreach(KeyValuePair<Tile, float> tile in distance)
        {
            if(tile.Value < minVal)
            {
                minVal = tile.Value;
                min = tile.Key;
            }
        }

        Debug.Log("Safest tile: " + min.index.ToString());

        if(!canFly) return GetPathRunable(position, min);
        else        return GetPathFlyable(position, min);
    }

    public List<Tile> FilterWaypointsByAttackRange(List<Tile> waypoints, Tile character, float range)
    {
        List<Tile> filtered = new List<Tile>();

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = gridManager.Distance(waypoints[i], character);

            if (distance >= range)
            {
                filtered.Add(waypoints[i]);
            }
            else
                break;
        }

        return filtered;
    }
}
