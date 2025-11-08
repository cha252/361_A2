using UnityEngine;

public class rotatingLaserScript : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of the rotation
    private Vector3 bottomEndPosition;

    void Start()
    {
        // Assuming the cylinder is oriented along the Y-axis and its pivot is in the center
        // Set the position of the bottom end of the cylinder (for example, the bottom end would be at (0, -height/2, 0))
        bottomEndPosition = transform.position - new Vector3(0, transform.localScale.y / 2, 0);
    }

    void Update()
    {
        // Rotate around the bottom end (around the Y-axis)
        transform.RotateAround(bottomEndPosition, Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
