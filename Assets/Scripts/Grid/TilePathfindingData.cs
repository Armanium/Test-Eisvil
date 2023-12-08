using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TilePathfindingData
{
    public Tile parent;

    public float distance;
    public float moveCount;

    public float total { get { return distance + moveCount; } }
}
