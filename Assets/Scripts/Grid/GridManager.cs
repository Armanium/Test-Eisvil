using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

public class GridManager : MonoBehaviour
{
    public List<string> vals = new List<string>();
    public static GridManager instance;

    public GameObject tilePrefab;

    public int sizeX;
    public int sizeY;
    public float scale;

    private Transform gridTransform;

    private Dictionary<TileIndex, Tile> grid = new Dictionary<TileIndex, Tile>();

    private TileIndex[] diagonalDirections = new TileIndex[]
        {
            new TileIndex(0, 1),
            new TileIndex(1, 1),
            new TileIndex(1, 0),
            new TileIndex(1, -1),
            new TileIndex(0, -1),
            new TileIndex(-1, -1),
            new TileIndex(-1, 0),
            new TileIndex(-1, 1),
        };

    private TileIndex[] crossDirections = new TileIndex[]
    {
            new TileIndex(0, 1),
            new TileIndex(1, 0),
            new TileIndex(0, -1),
            new TileIndex(-1, 0)
    };

    private string indexString;
    private TileIndex tempIndex = new TileIndex();
    private List<Tile> tempList = new List<Tile>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        int max = Mathf.Max(sizeY, sizeX);

        for (int i = 0; i < max; i++)
        {
            vals.Add(i.ToString());
        }


        gridTransform = this.transform;

        LoadMap();
    }

    public void InitalizeMap()
    {
        ClearMap();
        float offsetX = sizeX / 2;
        float offsetY = sizeY / 2;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                GameObject tileObject = Instantiate(tilePrefab, new Vector3((x * scale) - offsetX, 0, y * scale), Quaternion.Euler(new Vector3(90,0,0)));

                Tile tile = tileObject.GetComponent<Tile>();
                tile.Initalize(x, y, false);

                tileObject.name = GetTileString(tile.index);

                tileObject.transform.SetParent(gridTransform,false);

                grid.Add(tile.index, tile);
            }
        }
    }

    public void SaveMap()
    {
        List<TileData> tiles = new List<TileData>();
        foreach(Tile tile in grid.Values)
        {
            tiles.Add(new TileData
                {
                index = tile.index,
                isObstacle = tile.isObstacle
            });
        }

        string serialized = JsonConvert.SerializeObject(tiles);

        System.IO.File.WriteAllText(Application.dataPath + "/Map.json", serialized);
    }

    public void LoadMap()
    {
        ClearMap();

        TextAsset text = Resources.Load<TextAsset>("Maps/Map");

        string serialized = text.text;

        List<TileData> tiles = JsonConvert.DeserializeObject<List<TileData>>(serialized);

        tiles.Sort((x1, x2) => x2.index.y.CompareTo(x1.index.y));
        sizeY = tiles[0].index.y + 1;
        tiles.Sort((x1, x2) => x2.index.x.CompareTo(x1.index.x));
        sizeX = tiles[0].index.x + 1;

        float offsetX = sizeX / 2;
        float offsetY = sizeY / 2;

        foreach (TileData data in tiles)
        {
            GameObject tileObject = Instantiate(tilePrefab, new Vector3((data.index.x * scale) - offsetX, 0, data.index.y * scale), Quaternion.Euler(new Vector3(90, 0, 0)));

            Tile tile = tileObject.GetComponent<Tile>();

            tile.Initalize(data.index.x, data.index.y, data.isObstacle);


            tileObject.name = tile.index.ToString();

            tileObject.transform.SetParent(gridTransform, false);

            grid.Add(tile.index, tile);
        }
    }

    
    /// <summary>
    /// Ќаходит все существующие соседние (4 направлени€) плитки
    /// </summary>
    /// <param name="tile"></param>
    /// <returns>лист Tile</returns>
    public List<Tile> GetNeighboursDiagonal(Tile tile)
    {
        if (tile == null) return null;

        tempList.Clear();

        
        for (int i = 0; i < 8; i++)
        {
            tempIndex = tile.index + diagonalDirections[i];

            if (grid.ContainsKey(tempIndex))
            {
                tempList.Add(grid[tempIndex]);
            }
        }

        Debug.Log(tempList.Count);
        return tempList;
    }

    /// <summary>
    /// Ќаходит все существующие (8 направлени€) соседние плитки
    /// </summary>
    /// <param name="tile"></param>
    /// <returns>лист Tile</returns>
    public List<Tile> GetNeighboursCross(Tile tile)
    {
        if (tile == null) return null;

        tempList.Clear();

        for (int i = 0; i < 4; i++)
        {
            tempIndex = tile.index + crossDirections[i];

            if (grid.ContainsKey(tempIndex))
            {
                tempList.Add(grid[tempIndex]);
            }
        }
        return tempList;
    }
    /// <summary>
    /// Ќаходит все существующие плитки в радиусе
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="range"></param>
    /// <returns>лист Tile</returns>
    public List<Tile> GetRange(Tile tile, int range)
    {
        tempList.Clear();

        int xRange = Mathf.Clamp(range, -sizeX, sizeX);
        int yRange = Mathf.Clamp(range, -sizeY, sizeY);

        for (int x = -xRange; x <= xRange; x++)
        {
            for (int y = -yRange; y <= yRange; y++)
            {
                //Debug.Log("tempindex x,y = " + tempIndex.x +", " + tempIndex.y + ", X,Y = " + x + ", "+ y + ", Tile X,Y: " + tile.index.x + ", " + tile.index.y);
                tempIndex.x = x + tile.index.x;
                tempIndex.y = y + tile.index.y;

                if (tempIndex.x >= 0 && tempIndex.x < sizeX && tempIndex.y >= 0 && tempIndex.y < sizeY)
                {
                    tempList.Add(grid[tempIndex]);
                }
            }
        }
        return tempList;
    }
    public bool distancedebug = false;
    /// <summary>
    /// ƒистанци€ от одной плитки до другой
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>float</returns>
    public float Distance(Tile a, Tile b)
    {
        if (a == null) Debug.Log("A Tile is null");
        if (b == null) Debug.Log("B Tile is null");
        if (!distancedebug)
        {
            float d = Mathf.Abs(a.index.x - b.index.x) + Mathf.Abs(a.index.y - b.index.y);
            Debug.Log("A: " + GetTileString(a.index) + ", B: " + GetTileString(b.index) + ", Distance: " + d.ToString());
            distancedebug = true;
        }
        int x = Mathf.Abs(a.index.x - b.index.x);
        int y = Mathf.Abs(a.index.y - b.index.y);
        int c = Mathf.Min(x, y);
        return x + y - c;
    }

    
    public Tile PosToTile(Vector3 pos)
    {
        float x = pos.x;
        float z = pos.z;

        float _x = x + sizeX / 2;

        tempIndex.x = (int)Mathf.Round(_x);
        tempIndex.y = (int)Mathf.Round(z);

        if (grid.ContainsKey(tempIndex))
            return grid[tempIndex];
        else
            return null;
    }
    /*
    public Tile GetTileByString(string index)
    {
        if (grid.ContainsKey(index))
        {
            return grid[index];
        }

        return null;
    }*/

    public Tile GetTileByIndex(TileIndex index)
    {
        if (grid.ContainsKey(index)) return grid[index];
        else                         return null;
    }

    public void ClearTilePathfindingData()
    {
        foreach (Tile tile in grid.Values)
        {
            tile.tilePathfindingData.parent = null;
            tile.tilePathfindingData.distance = 0;
            tile.tilePathfindingData.moveCount = 0;
        }
    }

    private void ClearMap()
    {
        grid.Clear();

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    public string GetTileString(TileIndex index)
    {
        if(index.x < 0 || index.x >= sizeX || index.y < 0 || index.y >= sizeY)
            return "";

        StringBuilder i = new StringBuilder();
        i.Append("[");
        i.Append(vals[index.x]);
        i.Append(",");
        i.Append(vals[index.y]);
        i.Append("]");
        return i.ToString();
    }
}
