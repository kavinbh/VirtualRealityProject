using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private bool isRotating = true; // Orbit is active by default

    void Update()
    {
        if (isRotating)
        {
            RotateObject();
        }
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void ToggleOrbit()
    {
        isRotating = !isRotating; // Toggle rotation on/off
    }
}