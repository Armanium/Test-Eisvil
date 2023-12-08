using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    public override void Initalize()
    {
        base.Initalize();

        attackCooldown = attackSpeed;
    }
    public override void Tick()
    {
        base.Tick();

        animator.SetFloat("Attack speed", attackCooldown);
    }

    public override void AttackCharacter()
    {
        base.AttackCharacter();

        StartCoroutine(Cooldown());
    }

    public override void SetState(State _state)
    {
        base.SetState(_state);

        if (_state.stateName == "Attack")
        {
            _state.condition = AttackLogic;
            _state.update = UpdateLogic;
        }
    }

    public void UpdateLogic()
    {
        state.condition();

        if (attackCooldown < 0) attackCooldown = attackSpeed;

        if(attackCooldown == attackSpeed)
        {
            AttackCharacter();
            StartCoroutine(Cooldown());
        }
    
    }

    public void AttackLogic()
    {
        Debug.Log("new attack logic");
        if (health <= 0)
            SetState(new Death());

        if (IsCharacterDead())
            SetState(new Idle());

        if (IsPositionChanged() && !range.Contains(characterPosition))
        {
            CalculatePathToCharacter();
            SetState(new Move());
        }

        if (deadRange.Contains(position))
        {
            CalculatePathToSafeTile();
            SetState(new Retreat());
        }
    }

}
