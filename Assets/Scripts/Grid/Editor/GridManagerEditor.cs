using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridManager grid = (GridManager)target;

        if(GUILayout.Button("Создать карту"))
        {
            grid.InitalizeMap();
        }

        if (GUILayout.Button("Сохранить карту"))
        {
            grid.SaveMap();
        }

        if (GUILayout.Button("Загрузить карту"))
        {
            grid.LoadMap();
        }
    }
}
