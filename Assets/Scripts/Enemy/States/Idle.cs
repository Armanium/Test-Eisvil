using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
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

            List<Tile> attackableRange = parent.GetRange();
            List<Tile> fovRange = parent.GetFOV();

            Tile characterPosition = parent.GetCharacterPosition();

            if (characterPosition == null) return;

            if (fovRange.Contains(characterPosition) && !parent.IsPositionChanged())
            {
                Debug.Log("Idle -> Move");
                parent.CalculatePathToCharacter();
                parent.SetState(new Move());
            }

            if (attackableRange.Contains(characterPosition))
            {
                parent.SetState(new Attack());
            }

        }
    }

    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        stateName = "Idle";
    }
}
