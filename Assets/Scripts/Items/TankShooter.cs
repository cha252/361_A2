using System.Collections;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject bomb_prefab;
    public Transform fire_point;
    public float fire_interval = 2.0f;
    public float bomb_speed = 30.0f;

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fire_interval);
            ShootBomb();
        }
    }

    void ShootBomb()
    {
        if (bomb_prefab != null && fire_point != null)
        {
            GameObject bomb = Instantiate(bomb_prefab, fire_point.position, fire_point.rotation); // set fire point as an empty object infront of the muzzle.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = fire_point.forward * bomb_speed;
            }
        }
    }
}