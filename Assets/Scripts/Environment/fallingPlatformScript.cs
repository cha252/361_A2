using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    //Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    // Break the Platform
    public void Drop()
    {
        //Get the Rigidbody Component
        Rigidbody rb = GetComponent<Rigidbody>();

        //Move the Constraints
        rb.constraints = RigidbodyConstraints.None;

        //Make the Platform fall downwards
        rb.AddForce(Vector3.down * 2.0f, ForceMode.VelocityChange);
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Drop platform");
        // Make the Platform fall (possibly)
        Drop();
    }
}
