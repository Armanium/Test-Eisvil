using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Attack : Behaviour
{
    public Enemy parent;
    public virtual void Attack(Enemy.AttackTypes type,Character character, float damage)
    {
        switch(type)
        {
            case Enemy.AttackTypes.melee:

                MeleeAttack(character, damage);

                break;

            case Enemy.AttackTypes.ranged:

                RangedAttack(character, damage);

                break;

            case Enemy.AttackTypes.throwable:


                break;

            default:

                Debug.Log("Не найден такой тип атаки");
                return;
        }
    }
    public virtual void MeleeAttack(Character character, float damage)
    {
        transform.LookAt(character.transform.position);
        character.GetDamage(damage);
    }

    public virtual void RangedAttack(Character character, float damage)
    {

    }

    public virtual void ThrowableAttack()
    {

    }

    public virtual void SetParent(Enemy enemy)
    {
        parent = enemy;
    }
}
