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

        if(GUILayout.Button("������� �����"))
        {
            grid.InitalizeMap();
        }

        if (GUILayout.Button("��������� �����"))
        {
            grid.SaveMap();
        }

        if (GUILayout.Button("��������� �����"))
        {
            grid.LoadMap();
        }
    }
}
