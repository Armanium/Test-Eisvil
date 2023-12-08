using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    public override void ConditionHandler()
    {

    }
    public override void OnEnter(Enemy enemy)
    {
        enemy.Death(enemy);
    }

}
