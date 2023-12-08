using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool active = false;
    public float speed;
    public Character character;

    private void FixedUpdate()
    {
        if(active)
        {
            if(transform.position != character.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, character.transform.position, speed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Вы подобрали копейку!");
            }

        }
    }
}
