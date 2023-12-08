using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retreat : State
{
    public override void Update()
    {
        if(update != null) update();
        else
        {
            ConditionHandler();

            parent.Move();
        }
    }
    public override void ConditionHandler()
    {
        if (condition != null) condition();
        else
        {
            if (parent == null)
            {
                Debug.LogError("Null Enemy reference");
                return;
            }

            if (parent.GetHealth() <= 0)
                parent.SetState(new Death());


            List<Tile> deadRange = parent.GetDeadRange();


            if(!deadRange.Contains(parent.GetPosition()))
            {
                Debug.Log("Enemy is out of deadRange");
                parent.SetState(new Attack());
            }
            else
            {
                Debug.Log("Enemy is in deadRange");
                if(parent.waypoints.Count == 0)
                {
                    Debug.Log("Retreat/Calculate safe path");
                    parent.CalculatePathToSafeTile();
                }
            }

            if(parent.IsCharacterDead())
            {
                parent.SetState(new Idle());
            }
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        stateName = "Retreat";
    }

    public override void OnExit()
    {

    }
}
