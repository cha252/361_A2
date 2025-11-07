using Unity.VisualScripting;
using UnityEngine;

public class RotatingScript : MonoBehaviour
{
    //Declare variables
    public Transform centerPoint;
    public float radius = 7.5f;
    public float speed = 30f;
    public float startAngle;
    private float angle;

    void Start()
    {
        //Set the initial angle of the platform
        angle = startAngle;
    }

    void FixedUpdate()
    {
        //Update the angle 
        angle += speed * Time.deltaTime;
        //'Reset' the angle if it is greater than 360
        if (angle > 360f) angle -= 360f;

        //Get angle in radians
        float rad = angle * Mathf.Deg2Rad;

        //Calculate and update the new position of the platform
        Vector3 offset = new Vector3(0f, Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        transform.position = centerPoint.position + offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
