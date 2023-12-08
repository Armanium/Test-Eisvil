using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_RangedAttack : B_Attack
{
    public override void Attack(Enemy.AttackTypes type, Character character, float damage)
    {
        base.Attack(type, character, damage);
    }

    public override void RangedAttack(Character character, float damage)
    {
        Vector3 direction = character.transform.position;
        direction.y = transform.position.y;

        transform.LookAt(direction);

        direction = transform.position;
        direction.y = 0.5f;

        var orbObj = Instantiate(Resources.Load("Weapons/Orb") as GameObject, direction,transform.rotation);

        Orb orb = orbObj.GetComponent<Orb>();

        orb.damage = parent.GetDamage();
    }
}
