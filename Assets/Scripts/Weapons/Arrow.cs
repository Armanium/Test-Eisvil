using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public Enemy enemy = null;
    public float speed = 40;
    public bool moving = true;

    private void Awake()
    {
        StartCoroutine(WaitForDestroy());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(moving)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Arrow")
        {
            Debug.Log("returned");
            return;
        }

            if (moving)
        {
            enemy = other.gameObject.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.SetHealth(damage);
                Destroy(gameObject);
            }
            else
            {
                moving = false;
            }
        }
        

        
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }
}
