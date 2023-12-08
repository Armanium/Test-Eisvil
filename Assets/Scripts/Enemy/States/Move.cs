using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : State
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

            if(parent.IsPositionChanged())
            {
                parent.CalculatePathToCharacter();
            }

            List<Tile> attackableRange = parent.GetRange();
            List<Tile> fovRange = parent.GetFOV();

            Tile characterPosition = parent.GetCharacterPosition();

            if (parent.GetHealth() <= 0)
                parent.SetState(new Death());

            if(parent.waypoints == null || parent.waypoints.Count == 0)
            {
                if (attackableRange.Contains(characterPosition))
                    parent.SetState(new Attack());
                if (fovRange.Contains(characterPosition))
                {
                    parent.CalculatePathToCharacter();
                }
                else
                {
                    parent.waypoints.Clear();
                    parent.SetState(new Idle());
                }
            }
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        parent = enemy;
        stateName = "Move";
        parent.SetAnimatorBool("Running", true);
    }

    public override void OnExit()
    {
        parent.SetAnimatorBool("Running", false);
    }


}
