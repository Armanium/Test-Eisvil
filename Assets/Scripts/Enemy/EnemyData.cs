using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Enemy.AttackTypes attackType;
    public Enemy.MovementTypes movementType;
    public float health;
    public float damage;
    public float speed;
    public float attackSpeed;
    public int fov;
    public int range;
    public int deadZone;
}
