using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrackingInteraction : MonoBehaviour
{
    public Transform lockCrosshair; // Reference to the Lock crosshair
    public Transform mainCamera; // Reference to the Main Camera
    public float interactionThreshold = 5f; // Angle threshold for interaction
    public LayerMask interactableLayer; // Layer mask for interactable objects

    private Renderer lockRenderer; // Renderer for toggling visibility
    private bool isInteracting = false; // Flag to prevent multiple interactions

    void Start()
    {
        // Get the Renderer component from the lock crosshair for visibility control
        lockRenderer = lockCrosshair.GetComponentInChildren<Renderer>();
        lockRenderer.enabled = false;
    }

    void Update()
    {
        // Align the crosshair with the camera's forward direction
        transform.position = mainCamera.position + mainCamera.forward * 5f; 
        transform.rotation = mainCamera.rotation;

        // Check if the user is looking at an interactable object
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, interactableLayer))
        {
            // Show the lock crosshair when an interactable object is looked at
            lockRenderer.enabled = true;

            // Position and rotate the lock crosshair
            lockCrosshair.position = transform.position;
            lockCrosshair.rotation = Quaternion.Euler(45, 0, 0); // Lock at a fixed 45-degree angle

            // Calculate the angle between the two crosshairs
            float angle = Quaternion.Angle(transform.rotation, lockCrosshair.rotation);

            // If the crosshairs align within the threshold, trigger the interaction
            if (angle < interactionThreshold && !isInteracting)
            {
                isInteracting = true;
                hit.transform.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            // Hide the lock crosshair when not looking at an interactable object
            lockRenderer.enabled = false;
            isInteracting = false;
        }
    }
}