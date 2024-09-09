using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Transform crosshair;  // Reference to the crosshair plane
    public Transform lockCrosshair;  // Reference to the lock crosshair
    public Camera mainCamera;  // Reference to the Main Camera

    // Interaction variables
    public float requiredAlignmentAngle = 10f;  // Angle within which interaction can occur
    private bool canInteract = false;  // Control when interaction can happen

    // Crosshair appearance variables
    public Color defaultCrosshairColor = Color.white;  // Default crosshair color
    public Color highlightCrosshairColor = Color.green;  // Crosshair color when looking at an interactable object
    private Renderer crosshairRenderer;  // Renderer for crosshair
    private Renderer lockCrosshairRenderer;  // Renderer for lock crosshair

    private MovableCube hoveredCube = null;

    void Start()
    {
        // Get references to the crosshair renderers (assuming the crosshairs are planes with materials)
        crosshairRenderer = crosshair.GetComponent<Renderer>();
        lockCrosshairRenderer = lockCrosshair.GetComponent<Renderer>();

        // Set crosshairs to their default colors initially
        crosshairRenderer.material.color = defaultCrosshairColor;
        lockCrosshairRenderer.material.color = defaultCrosshairColor;
    }

    // void Update()
    // {
    //     // Position crosshair in front of the camera
    //     crosshair.position = mainCamera.transform.position + mainCamera.transform.forward * 2.0f;
    //     crosshair.LookAt(mainCamera.transform);

    //     // Position lock crosshair in front of the camera with fixed orientation
    //     lockCrosshair.position = mainCamera.transform.position + mainCamera.transform.forward * 2.0f;
    //     lockCrosshair.LookAt(mainCamera.transform);

    //     // Roll the crosshair based on head movement
    //     crosshair.rotation = mainCamera.transform.rotation;

    //     // The lock crosshair has a fixed rotation (e.g., rotated by 45 degrees)
    //     lockCrosshair.rotation = Quaternion.Euler(0, 0, 45);  // Adjust if needed

    //     // Perform raycast to check for interactable objects
    //     Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
    //     RaycastHit hit;

    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         // Check if hit object is interactable
    //         if (hit.transform.CompareTag("Interactable"))
    //         {
    //             // Change crosshair color to highlight when looking at interactable object
    //             crosshairRenderer.material.color = highlightCrosshairColor;
    //             lockCrosshairRenderer.material.color = highlightCrosshairColor;

    //             // Show the lock crosshair and check for alignment
    //             lockCrosshair.gameObject.SetActive(true);

    //             // Calculate the angle between the crosshair and lockCrosshair
    //             float angleBetween = Quaternion.Angle(crosshair.rotation, lockCrosshair.rotation);

    //             if (angleBetween < requiredAlignmentAngle && !canInteract)
    //             {
    //                 // Interaction can occur now
    //                 canInteract = true;
    //                 InteractWithObject(hit.transform);  // Call interaction method and pass the object
    //             }

    //             // Reset interaction state when head is straightened again
    //             if (angleBetween > requiredAlignmentAngle + 10)  // Add a small buffer to reset
    //             {
    //                 canInteract = false;
    //             }
    //         }
    //         else
    //         {
    //             // Reset crosshair to default color if not looking at interactable object
    //             crosshairRenderer.material.color = defaultCrosshairColor;
    //             lockCrosshairRenderer.material.color = defaultCrosshairColor;
    //             lockCrosshair.gameObject.SetActive(false);
    //         }
    //     }
    //     else
    //     {
    //         // Reset crosshair when nothing is looked at
    //         crosshairRenderer.material.color = defaultCrosshairColor;
    //         lockCrosshairRenderer.material.color = defaultCrosshairColor;
    //         lockCrosshair.gameObject.SetActive(false);
    //     }
    // }

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

    // Perform raycast to check for interactable objects
    Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
        // Check if hit object is interactable
        if (hit.transform.CompareTag("Interactable"))
        {
            // Change crosshair color to highlight when looking at interactable object
            crosshairRenderer.material.color = highlightCrosshairColor;
            lockCrosshairRenderer.material.color = highlightCrosshairColor;

            // Show the lock crosshair and check for alignment
            lockCrosshair.gameObject.SetActive(true);

            // Calculate the angle between the crosshair and lockCrosshair
            float angleBetween = Quaternion.Angle(crosshair.rotation, lockCrosshair.rotation);

            if (angleBetween < requiredAlignmentAngle && !canInteract)
            {
                // Interaction can occur now
                canInteract = true;

                // If the object is a movable cube, handle hover and movement
                MovableCube movableCube = hit.transform.GetComponent<MovableCube>();
                if (movableCube != null)
                {
                    movableCube.OnHoverEnter();  // Trigger hover interaction for movable cube

                    // Here you can add logic for selecting the cube after a hover time
                    // The MovableCube script will handle the timing and movement once selected
                }
                else
                {
                    InteractWithObject(hit.transform);  // Call interaction method and pass the object
                }
            }

            // Reset interaction state when head is straightened again
            if (angleBetween > requiredAlignmentAngle + 10)  // Add a small buffer to reset
            {
                canInteract = false;
            }
        }
        else
        {
            // Reset crosshair to default color if not looking at interactable object
            crosshairRenderer.material.color = defaultCrosshairColor;
            lockCrosshairRenderer.material.color = defaultCrosshairColor;
            lockCrosshair.gameObject.SetActive(false);

            // Handle exiting hover for movable cube if no longer looking at it
            if (hoveredCube != null)
            {
                hoveredCube.OnHoverExit();  // Stop hover interaction for previous cube
                hoveredCube = null;
            }
        }
    }
    else
    {
        // Reset crosshair when nothing is looked at
        crosshairRenderer.material.color = defaultCrosshairColor;
        lockCrosshairRenderer.material.color = defaultCrosshairColor;
        lockCrosshair.gameObject.SetActive(false);

        // Handle exiting hover for movable cube when nothing is hit
        if (hoveredCube != null)
        {
            hoveredCube.OnHoverExit();  // Stop hover interaction for previous cube
            hoveredCube = null;
        }
    }
}
    void InteractWithObject(Transform interactableObject)
    {
        // Implement interaction logic for interactable object, for example, change color or move object
        Debug.Log("Interaction Occurred with " + interactableObject.name);

        // Example: Changing color of the object when interacted with
        Renderer objectRenderer = interactableObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.red;  // Change object color to red as an interaction example
        }
    }

    void ResetCrosshair()
    {
        crosshairRenderer.material.color = defaultCrosshairColor;
        lockCrosshairRenderer.material.color = defaultCrosshairColor;
        lockCrosshair.gameObject.SetActive(false);

        if (hoveredCube != null)
        {
            hoveredCube.OnHoverExit();  // Exit hover on the current cube
            hoveredCube = null;
        }
    }
}