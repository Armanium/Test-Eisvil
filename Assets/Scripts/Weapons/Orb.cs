using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float damage;
    public Character character = null;
    public float speed = 40;

    private void Awake()
    {
        Debug.Log("Orb");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Arrow") return;
            character = other.gameObject.GetComponent<Character>();

            if (character != null)
            {
                character.GetDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
}
