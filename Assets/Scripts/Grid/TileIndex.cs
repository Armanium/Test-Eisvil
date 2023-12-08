using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileIndex : IEquatable<TileIndex>
{
    public int x, y;
    public TileIndex(int x, int y)
    {
        this.x = x; this.y = y;
    }

    public static TileIndex operator +(TileIndex first, TileIndex second)
    {
        return new TileIndex(first.x + second.x, first.y + second.y);
    }

    public static TileIndex operator -(TileIndex first, TileIndex second)
    {
        return new TileIndex(first.x - second.x, first.y - second.y);
    }

    public static bool operator ==(TileIndex first, TileIndex second)
    {
        if (first.x == second.x && first.y == second.y) return true;
        else return false;
    }

    public static bool operator !=(TileIndex first, TileIndex second)
    {
        if (first.x != second.x || first.y != second.y) return true;
        else return false;
    }

    public override string ToString()
    {
        return string.Format("[" + x + "," + y + "]");
    }

    public bool Equals(TileIndex obj)
    {
        return obj is TileIndex index &&
               x == index.x &&
               y == index.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}
