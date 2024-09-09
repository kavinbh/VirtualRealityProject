using UnityEngine;

public class MovableCube : MonoBehaviour
{
    public float hoverTimeToSelect = 2.0f;  // Time to select cube for movement when hovered over
    public float hoverTimeToPlace = 2.0f;   // Time to place the cube when hovered over a new spot

    private bool isSelectedForMovement = false;  // Whether the cube is currently being moved
    private float hoverTimer = 0f;  // Timer to track how long the user has been looking at the cube or a location

    private Vector3 originalPosition;  // Stores the original position of the cube
    private Renderer cubeRenderer;

    private bool isBeingHovered = false;  // Check if the cube is being hovered by crosshairs

    private Color defaultColor = Color.white;
    private Color highlightColor = Color.green;

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeRenderer.material.color = defaultColor;
        originalPosition = transform.position;  // Store the initial position
    }

    void Update()
    {
        if (isBeingHovered && !isSelectedForMovement)
        {
            // Increment hover timer while the cube is hovered
            hoverTimer += Time.deltaTime;
            
            // If the cube is hovered long enough, select it for movement
            if (hoverTimer >= hoverTimeToSelect)
            {
                isSelectedForMovement = true;
                hoverTimer = 0f;  // Reset timer for movement placement
            }
        }
        else if (isSelectedForMovement)
        {
            // Move the cube to follow the player's gaze (look direction)
            Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);  // Smooth movement

            // When the player looks at a location for a while, place the cube
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverTimeToPlace)
            {
                PlaceCube();
            }
        }
        else
        {
            // Reset the hover timer when the user looks away
            hoverTimer = 0f;
        }
    }

    public void OnHoverEnter()
    {
        isBeingHovered = true;
        cubeRenderer.material.color = highlightColor;  // Change color to indicate interaction
    }

    public void OnHoverExit()
    {
        isBeingHovered = false;
        cubeRenderer.material.color = defaultColor;  // Reset to default color
        hoverTimer = 0f;  // Reset timer if user looks away
    }

    void PlaceCube()
    {
        isSelectedForMovement = false;  // Stop the movement
        hoverTimer = 0f;  // Reset timer
        Debug.Log("Cube placed at: " + transform.position);  // For debugging
    }
}