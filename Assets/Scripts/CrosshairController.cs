using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Transform crosshair;  // Reference to the crosshair plane
    public Transform lockCrosshair;  // Reference to the lock crosshair
    public Camera mainCamera;  // Reference to the Main Camera

    // Interaction variables
    public float requiredAlignmentAngle = 10f;  // Angle within which interaction can occur
    private bool canInteract = false;  // Control when interaction can happen

    void Update()
    {
        // Position crosshair in front of the camera
        crosshair.position = mainCamera.transform.position + mainCamera.transform.forward * 2.0f;
        crosshair.LookAt(mainCamera.transform);

        // Position lock crosshair in front of the camera with fixed orientation
        lockCrosshair.position = mainCamera.transform.position + mainCamera.transform.forward * 2.0f;
        lockCrosshair.LookAt(mainCamera.transform);

        // Roll the crosshair based on head movement
        crosshair.rotation = mainCamera.transform.rotation;

        // The lock crosshair has a fixed rotation (e.g., rotated by 45 degrees)
        lockCrosshair.rotation = Quaternion.Euler(0, 0, 45);  // Adjust if needed

        // Calculate the angle between the crosshair and lockCrosshair
        float angleBetween = Quaternion.Angle(crosshair.rotation, lockCrosshair.rotation);

        if (angleBetween < requiredAlignmentAngle && !canInteract)
        {
            // Interaction can occur now
            canInteract = true;
            InteractWithObject();  // Call your interaction method
        }

        // Reset interaction state when head is straightened again
        if (angleBetween > requiredAlignmentAngle + 10)  // Add a small buffer to reset
        {
            canInteract = false;
        }

        // Perform raycast to check for interactable objects
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if hit object is interactable
            if (hit.transform.CompareTag("Interactable"))
            {
                lockCrosshair.gameObject.SetActive(true);  // Show the lock crosshair
            }
            else
            {
                lockCrosshair.gameObject.SetActive(false);  // Hide the lock crosshair
            }
        }
        else
        {
            lockCrosshair.gameObject.SetActive(false);  // Hide when nothing is looked at
        }
    }

    void InteractWithObject()
    {
        // Implement interaction logic, for example, change color or move object
        Debug.Log("Interaction Occurred!");

        // Example: Changing color
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Interactable"))
        {
            Renderer objectRenderer = hit.transform.GetComponent<Renderer>();
            objectRenderer.material.color = Color.red;  // Change color to red
        }
    }
}