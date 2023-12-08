using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRunable
{
    void Move(Transform transform, float speed, Vector3 destionation);
    List<Tile> GetMovePath(Tile origin, Tile destination);
}
