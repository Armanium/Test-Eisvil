using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public override void Update()
    {
        if (update != null) update();
        else
        {
            ConditionHandler();
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

            if(parent.IsCharacterDead())
                parent.SetState(new Idle());

            if (parent.IsPositionChanged() && !parent.GetRange().Contains(parent.GetCharacterPosition()))
            {
                parent.CalculatePathToCharacter();
                parent.SetState(new Move());
            }
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        stateName = "Attack";
        parent.SetAnimatorBool("Attacking", true);
    }

    public override void OnExit()
    {
        parent.SetAnimatorBool("Attacking", false);
    }
}
