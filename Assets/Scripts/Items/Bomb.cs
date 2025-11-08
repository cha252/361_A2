using UnityEngine;

// Please read the comments if you are certain about something before you further develop on it - jy. 

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float life_time = 5.0f;
    public float kb_force = 15.0f; // kb -> knockback

    [Header("Effects")]
    public GameObject explosion_effect; // imported asset 
    public float effectLifetime = 2.0f;
    private bool explosion_flag = false;

    void Start()
    {
        Destroy(gameObject, life_time);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explosion_flag) return;
        if (collision.gameObject.CompareTag("Player")) // I've already put the tag. 
        {
            Explode(collision.gameObject);
        }
    }

    void Explode(GameObject player)
    {
        explosion_flag = true;
        PlayerScript2 playerScript = player.GetComponent<PlayerScript2>();

        if (playerScript != null) // nockback calcuation occurs here!
        {
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            playerScript.GetKnockedBack(knockbackDirection, kb_force);
        }
        if (explosion_effect != null)
        {
            GameObject effect = Instantiate(explosion_effect, transform.position, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
        Destroy(gameObject);  // removes the bomb after the certain amount of time. 
    }
}