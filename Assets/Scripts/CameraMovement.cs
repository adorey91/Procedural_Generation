using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float rotationSpeed = 10f;
    public float distanceFromCenter = 20f;
    public CityGenerator cityGenerator;

    private Vector3 center;

    void Start()
    {
        // Calculate the center of the grid
        center = new Vector3((cityGenerator.GridWidth() - 1) / 2, 0, (cityGenerator.GridLength() - 1) / 2);

        // Position the camera at the desired distance from the center
        transform.position = center + new Vector3(0, distanceFromCenter, -distanceFromCenter);
        transform.LookAt(center);
    }

    void Update()
    {
        // Rotate the camera around the center point at a set speed
        transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime);
        transform.LookAt(center); // Keep the camera looking at the center
    }
}