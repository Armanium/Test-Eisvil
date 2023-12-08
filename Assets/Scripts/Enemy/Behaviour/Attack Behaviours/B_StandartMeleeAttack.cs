using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_StandartMeleeAttack : B_Attack
{
    public override void Attack(Enemy.AttackTypes type, Character character, float damage)
    {
        switch (type)
        {
            case Enemy.AttackTypes.melee:

                MeleeAttack(character, damage);

                break;

            case Enemy.AttackTypes.ranged:


                break;

            case Enemy.AttackTypes.throwable:


                break;

            default:

                Debug.Log("Не найден такой тип аттаки");
                return;
        }
    }
    public override void MeleeAttack(Character character, float damage)
    {
        Debug.Log("Meele Attack");
        transform.LookAt(character.transform.position);
        character.GetDamage(damage);
    }
}
