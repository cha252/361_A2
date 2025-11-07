using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movigBlockScript : MonoBehaviour
{
    //Declare variables
    public float speed = 2.5f;
    public float minZ = 45.0f;
    public float maxZ = 65.0f;

    private float startY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float range = maxZ - minZ;
        float newZ = Mathf.PingPong(Time.time * speed, range) + minZ;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}