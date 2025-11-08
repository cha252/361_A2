using Unity.VisualScripting;
using UnityEngine;

public class ColumnPlatformScript : MonoBehaviour
{
    //Declare variables
    public Transform waypoint1, waypoint2, waypoint3, waypoint4;
    public float speed = 2.0f;
    private Transform currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the initial target to waypoint1
        currentTarget = waypoint2;
        // Set initial platform position
        transform.position = waypoint2.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move towards the current target waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        //if the platform has reached the target waypoint
        if (transform.position == currentTarget.position)
        {
            //Determine the next waypoint
            if (currentTarget == waypoint1)
            {
                currentTarget = waypoint2;
            }
            else if (currentTarget == waypoint2)
            {
                currentTarget = waypoint3;
            }   
            else if (currentTarget == waypoint3)
            {
                currentTarget = waypoint4;
            }   
            else if(currentTarget == waypoint4)
            {
                currentTarget = waypoint1;
            }   
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Make the platfrom the parent of the player
        other.transform.parent = transform;
    }

    void OnTriggerExit(Collider other)
    {
        //Remove the platform as the parent of the player
        other.transform.SetParent(null);
    }
}