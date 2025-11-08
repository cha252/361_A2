using System.Collections;
using UnityEngine;
// Ground spike obstacle(cube) that pops up from the ground periodically
public class GroundSpike : MonoBehaviour
{
    [Header("Spike Settings")]
    public float raiseHeight = 2.0f;
    public float raiseSpeed = 11.0f;
    public float stayDuration = 1.0f;
    public float lowerSpeed = 11.0f;
    public float idleDuration = 1.0f;
    [Header("Knockback Settings")]
    private Vector3 loweredPosition;
    private Vector3 raisedPosition;
    private bool isMoving = false;
    void Start()
    {
        loweredPosition = transform.position; // jy - store initial position as lowered state
        raisedPosition = loweredPosition + Vector3.up * raiseHeight;
        StartCoroutine(SpikeCycle()); // start the spike cycle! jy
    }
    IEnumerator SpikeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleDuration);

            yield return StartCoroutine(RaiseSpike()); // raise the obstacle (spike) 
            yield return new WaitForSeconds(stayDuration);
            yield return StartCoroutine(LowerSpike());
        }
    }
    IEnumerator RaiseSpike()
    {
        isMoving = true;
        while (transform.position.y < raisedPosition.y - 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                raisedPosition,
                raiseSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = raisedPosition;
        isMoving = false;
    }
    IEnumerator LowerSpike()
    {
        isMoving = true;
        while (transform.position.y > loweredPosition.y + 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                loweredPosition,
                lowerSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = loweredPosition;
        isMoving = false;

        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Spike hit: " + collision.gameObject.name); // for debugging
    }
}