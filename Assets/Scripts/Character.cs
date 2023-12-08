using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Character : MonoBehaviour
{
    public static Character instance = null;

    private Joystick joystick;
    private GameManager gameManager;

    public float health;
    public float damage;
    public float speed;
    public float attackSpeed;
    public float attackCooldown;
    public int attackRange;

    [SerializeField] private Enemy target;
    public Tile position;
    public List<Tile> range {  get; private set; }

    public bool isDead = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        joystick = FindObjectOfType<Joystick>();
        gameManager = FindObjectOfType<GameManager>();

        attackCooldown = attackSpeed;
    }

    private void FixedUpdate()
    {
        target = gameManager.GetNearestEnemy();
        if(health <= 0)
        {
            gameManager.DeathUI();
        }
        if(!joystick.IsCentered())
        {
            Move();
        }
        else
        {
            if(target != null)
            {
                if(target)
                if(attackCooldown == attackSpeed)
                    {
                        Attack();
                        StartCoroutine(Cooldown());
                    }
            }
            else
            {
                
            }
        }
    }

    private IEnumerator Cooldown()
    {
        while (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        attackCooldown = attackSpeed;
    }

    private void Move()
    {

        FaceDirection(joystick.direction);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void Attack()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.LookAt(transform.position + dir.normalized);

        Arrow arrow = Instantiate(Resources.Load("Weapons/Arrow") as GameObject,
                                    transform.position,
                                    transform.rotation).GetComponent<Arrow>();
        arrow.damage = damage;
    }

    private void Die()
    {
        Debug.Log("Dead");
    }

    private void FaceDirection(Vector3 direction)
    {
        Vector3 dir = transform.position + direction;
        dir.y = transform.position.y;

        transform.LookAt(dir);
    }

    public void SetPosition(Tile tile)
    {
        position = tile;
    }

    public void SetRange(List<Tile> tileRange)
    {
        range = tileRange;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }

    public int GetRange()
    {
        return attackRange;
    }

}
