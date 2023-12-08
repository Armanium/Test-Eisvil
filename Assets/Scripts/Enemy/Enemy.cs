using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected GameManager gameManager;

    public EnemyData data;

    public bool active;
    [SerializeField] protected State state = null;

    protected Character character;
    protected Animator animator;

    [SerializeField] protected Movement movement;
    [SerializeField] protected B_Attack attack;

    public enum AttackTypes
    {
        melee,
        ranged,
        throwable
    }

    public enum MovementTypes
    {
        run,
        fly
    }

    [SerializeField] protected AttackTypes attackType;
    [SerializeField] protected MovementTypes movementType;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int attackRange;
    [SerializeField] protected int fov;
    [SerializeField] protected int deadZone;

    [SerializeField] protected Tile position;
    [SerializeField] protected Tile previousPosition;
    [SerializeField] protected Tile characterPosition;
    [SerializeField] protected List<Tile> range;
    [SerializeField] protected List<Tile> fovRange;
                     public List<Tile> waypoints;
                     public List<Tile> deadRange;
                     public AnimationCurve flypath;
                     public float flypathSpeed;
                     public float flytime;

    public virtual void Initalize()
    {
        attack.parent = this;

        attackType = data.attackType; 
        movementType = data.movementType;

        health = data.health;
        damage = data.damage;
        speed = data.speed;
        attackSpeed = data.attackSpeed;
        attackRange = data.range;
        fov = data.fov;
        deadZone = data.deadZone;

        SetState(new Idle());

        character = FindObjectOfType<Character>();
        animator = GetComponent<Animator>();
    }

    public virtual void Tick()
    {
        if(active)
            state.Update();
    }


    public virtual void SetHealth(float damage) 
    {
        health -= damage;

        animator.SetTrigger("Damage");
    }

    public virtual float GetHealth()
    {
        return health;
    }

    public virtual float GetDamage()
    {
        return damage;
    }    

    public virtual void SetPosition(Tile pos)
    {
        position = pos;
    }

    public virtual Tile GetPosition()
    {
        return position;
    }

    public virtual void SetDeadRange(GridManager grid)
    {
        deadRange.Clear();
        deadRange.AddRange(grid.GetRange(characterPosition, deadZone));
    }

    public virtual List<Tile> GetDeadRange()
    {
        return deadRange;
    }


    public virtual void SetRange(GridManager grid)
    {
        range.Clear();
        List<Tile> asd = grid.GetRange(position, attackRange);
        Debug.Log(asd.Count);

        range.AddRange(grid.GetRange(position, attackRange));
    }

    public virtual List<Tile> GetRange()
    {
        return range;
    }

    public virtual void SetChararcterPosition(Tile pos)
    {
        characterPosition = pos;
    }

    public virtual Tile GetCharacterPosition()
    {
        return characterPosition;
    }

    public UnityEvent<Enemy> deathEvent;
    public virtual void Death(Enemy enemy)
    {
        deathEvent.Invoke(enemy);

        var a = Instantiate(Resources.Load("Coin") as GameObject, transform.position, Quaternion.identity);

        a.GetComponent<Coin>().character = character;
    }

    public virtual void SetFOV(GridManager grid)
    {
        fovRange.Clear();
        fovRange.AddRange(grid.GetRange(position, fov));
    }

    public virtual List<Tile> GetFOV()
    {
        return fovRange;
    }

    public virtual void SetState(State _state)
    {
        State prevState = state;
        state = _state;

        if (prevState != null) prevState.OnExit();
        state.OnEnter(this);
    }

    public virtual void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    public virtual void CalculatePathToCharacter()
    {
        if(movementType == MovementTypes.run)
        {
            waypoints = gameManager.GetMoveablePath(position);
        }
        else
        {
            waypoints = gameManager.GetFlyablePath(position);
        }

        waypoints = gameManager.FilterByAttackingRange(waypoints, attackRange);
    }

    public virtual void CalculatePathToSafeTile()
    {
        waypoints = gameManager.GetPathToSafeTile(deadRange, position,deadZone,false);
        //waypoints = gameManager.FilterByAttackingRange(waypoints, deadZone);
    }

    public virtual void AttackCharacter()
    {
        attack.Attack(attackType, character, damage);
    }

    public virtual void Move()
    {
        movement.Move(movementType, transform, speed, waypoints);
    }

    public virtual void SetAnimatorBool(string name, bool val)
    {
        animator.SetBool(name, val);
    }

    public virtual bool IsCharacterDead()
    {
        return character.isDead;
    }

    public virtual bool IsPositionChanged()
    {
        if (characterPosition != previousPosition)
        {
            previousPosition = characterPosition;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual IEnumerator Cooldown()
    {
        int iter = 0;
        while(attackCooldown > 0)
        {
            iter++;
            if (iter > 1000)
            {
                Debug.Log("Слишком длинный цикл");

                yield break;
            }

            attackCooldown -= Time.deltaTime;
            animator.SetFloat("Attack speed", attackCooldown);

            yield return new WaitForFixedUpdate();
        }
    }

}
