using System.Collections;
using UnityEngine;

// Ground spike obstacle(cube) that pops up from the ground periodically
public class GroundSpike : MonoBehaviour
{
    [Header("Spike Settings")]
    public float raise_height = 2.0f; 
    public float raise_speed = 11.0f; 
    public float stay_duration = 1.0f; 
    public float lower_speed = 11.0f; 
    public float idle_duration = 1.0f; 

    [Header("Knockback Settings")]
    public float kb_force = 12.0f; // jy - knockback force when player hits spike

    [Header("Warning (Optional)")]
    public GameObject warning_indicator; // jy - visual warning before spike raises
    public float warning_duration = 0.5f; // jy - warning duration

    private Vector3 lowered_position;
    private Vector3 raised_position;
    private bool is_moving = false;

    void Start()
    {
        lowered_position = transform.position; // jy - store initial position as lowered state
        raised_position = lowered_position + Vector3.up * raise_height;
        StartCoroutine(SpikeCycle()); // start the spike cycle! jy
    }

    IEnumerator SpikeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(idle_duration);

            
            yield return StartCoroutine(RaiseSpike()); // raise the obstacle (spike) 
            yield return new WaitForSeconds(stay_duration);
            yield return StartCoroutine(LowerSpike());
        }
    }

    IEnumerator RaiseSpike()
    {
        is_moving = true;

        while (transform.position.y < raised_position.y - 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                raised_position,
                raise_speed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = raised_position;
        is_moving = false;
    }

    IEnumerator LowerSpike()
    {
        is_moving = true;

        while (transform.position.y > lowered_position.y + 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                lowered_position,
                lower_speed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = lowered_position;
        is_moving = false;
    }

    void OnCollisionEnter(Collision collision)
    {
    }
}