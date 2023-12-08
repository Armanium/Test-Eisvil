using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollow : MonoBehaviour
{
    public Vector3 offset;
    public float start;
    public float end;
    Transform character;
    public Transform boundTop;
    public Transform boundBottom;
    public Transform boundLeft;
    public Transform boundRight;
    Camera cam;
    public float side;
    public float orthographicSize;

    private void Awake()
    {
        cam = Camera.main;
        character = FindObjectOfType<Character>().transform;
    }

    private void Update()
    {
        float ratio = (float)Screen.height / Screen.width;
        orthographicSize = (Mathf.Abs(boundLeft.position.x) * ratio) * side;
        cam.orthographicSize = orthographicSize;

        end = boundTop.position.z - (orthographicSize * 1.4142f) * 2f;
        start = boundBottom.position.z + 5.05f;

        transform.position = new Vector3(transform.position.x, orthographicSize * 1.4142f, character.position.z + offset.z);
        if (transform.position.z > end || transform.position.z < start)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, start, end));
        }

        /*
            Vector3 pos = new Vector3();
            pos.z = (character.position.z + -45f * Mathf.Cos(0)) + offset.z;

            pos.z = Mathf.Clamp(pos.z, start, end.z);

            transform.position = pos;
        */
    }
}
