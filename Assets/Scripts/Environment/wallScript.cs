using UnityEngine;

public class wallScript : MonoBehaviour
{
    //Declare variables
    public float speed = 2.5f;
    public float minY = -1.5f;
    public float maxY = 4f;
    
    private float startY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float range = maxY - minY;
        float newY = Mathf.PingPong(Time.time * speed, range) + minY;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
