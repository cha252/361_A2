// using System.Collections;
// using UnityEngine;

// public class PlatformScript : MonoBehaviour {
//     // Start is called before the first frame update
//     void Start(){
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
    
//     //Function to make the platofrm wait for a bit before falling
//     private IEnumerator DropDelay()
//     {
//         yield return new WaitForSeconds(1);

//         // Get the Rigidbody Component
//         Rigidbody rb = GetComponent<Rigidbody>();
//         rb.isKinematic = false;

//         // Move the Constraints
//         rb.constraints = RigidbodyConstraints.None;

//         // Make the Platform fall downwards
//         rb.AddForce(Vector3.down * 2.0f, ForceMode.VelocityChange);

//     }

//     // Break the Platform
//     public void Break()
//     {
//         StartCoroutine(DropDelay());
//     }   

//     public void ShowLight() {
//         // Get Platform Spotlight
//         GameObject light = transform.Find("Light").gameObject;

//         // Set Light to Active
//         light.SetActive(true);
//     }

//     public void HideLight() {
//         // Get Platform Spotlight
//         GameObject light = transform.Find("Light").gameObject;

//         // Set Light to Inactive
//         light.SetActive(false);
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         //If collision is the player
//         if (other.CompareTag("Player"))
//         {
//             Break();
//         }
//     }   
// }


using System.Collections;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isFalling = false;

    void Start()
    {
        // Cache the Rigidbody and starting transform
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Make sure it starts as static/kinematic
        rb.isKinematic = true;
    }

    void Update()
    {
        // Check if platform has fallen below Y = -5
        if (isFalling && transform.position.y < -5f)
        {
            Respawn();
        }
    }

    private IEnumerator DropDelay()
    {
        yield return new WaitForSeconds(1f);

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.down * 2.0f, ForceMode.VelocityChange);
        isFalling = true;
    }

    public void Break()
    {
        StartCoroutine(DropDelay());
    }

    private void Respawn()
    {
        // Stop physics and reset transform
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Reset state
        isFalling = false;
    }

    public void ShowLight()
    {
        GameObject light = transform.Find("Light").gameObject;
        light.SetActive(true);
    }

    public void HideLight()
    {
        GameObject light = transform.Find("Light").gameObject;
        light.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Break();
        }
    }
}
