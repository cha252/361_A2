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

    //Method to drop the platform half a second after the player runs over it
    private IEnumerator DropDelay()
    {
        //Delay before the platform drops
        yield return new WaitForSeconds(0.5f);

        //Drop the platform
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.down * 2.0f, ForceMode.VelocityChange);
        isFalling = true;
    }
    //Method to call the coroutine to drop the platform with a delay
    public void Drop()
    {
        StartCoroutine(DropDelay());
    }

    //Method to get the platform to reappear after it falls past a certain height
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

    //Method to detect the player walking on the platform
    void OnTriggerEnter(Collider other)
    {
        //Drop the platform if the collision with the platform is the player
        if (other.CompareTag("Player"))
        {
            Drop();
        }
    }
}
