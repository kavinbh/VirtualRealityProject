using UnityEngine;
public class HeadTrackingInteraction : MonoBehaviour
{
    public Transform crosshair; 
    public Transform lockCrosshair;
    public Transform mainCamera;
    private bool isInteracting = false;
    private float interactionAngleThreshold = 5f;

    void Start()
    {
        crosshair.position = mainCamera.position + mainCamera.forward * 2.0f;
        lockCrosshair.position = mainCamera.position + mainCamera.forward * 2.0f;
    }

    void Update()
    {
        HandleCrosshairMovement();
        CheckForInteraction();
    }

    void HandleCrosshairMovement()
    {
        crosshair.position = mainCamera.position + mainCamera.forward * 2.0f;
        lockCrosshair.position = mainCamera.position + mainCamera.forward * 2.0f;
        crosshair.rotation = mainCamera.rotation;
        lockCrosshair.rotation = Quaternion.Euler(45, 0, 0) * mainCamera.rotation;
    }

    void CheckForInteraction()
    {
        float angleDifference = Quaternion.Angle(crosshair.rotation, lockCrosshair.rotation);
        if (angleDifference <= interactionAngleThreshold && !isInteracting)
        {
            isInteracting = true;
            TriggerInteraction();
        }
        if (angleDifference > interactionAngleThreshold)
        {
            isInteracting = false;
        }
    }

    void TriggerInteraction()
    {
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                // Call interaction methods based on the attached scripts
                InteractableObject interactable = hit.transform.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    interactable.OnInteract(); // Perform the interaction
                }

                Orbit orbit = hit.transform.GetComponent<Orbit>();
                if (orbit != null)
                {
                    orbit.ToggleOrbit(); // Toggle the orbiting behavior if the object has an Orbit script
                }
            }
        }
    }
}