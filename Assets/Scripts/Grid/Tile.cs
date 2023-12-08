using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public TileIndex index;
    public TilePathfindingData tilePathfindingData;

    public Material lightGreen;
    public Material darkGreen;
    public Material obstacle;

    public Renderer rend;

    public bool isObstacle;

    public void Initalize(int x, int y, bool o)
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();

        index = new TileIndex(x, y);
        isObstacle = o;

        if(isObstacle)
        {
            rend.material = obstacle;

            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        else if ((x + y) % 2 == 0)
        {
            rend.material = lightGreen;
        }
        else { rend.material = darkGreen;}

    }
}
