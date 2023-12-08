using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    public string stateName;
    public Enemy parent;

    public delegate void ConditionDelegate();
    public ConditionDelegate condition;
    public virtual void ConditionHandler()
    {
    }

    public delegate void UpdateDelegate();
    public UpdateDelegate update;
    public virtual void Update()
    {
    }

    public virtual void OnEnter(Enemy enemy)
    {
        parent = enemy;
    }

    public virtual void OnExit()
    {
    }
}
