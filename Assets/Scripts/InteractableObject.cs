using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractionType { ChangeColor, Move, Scale };
    public InteractionType interactionType = InteractionType.ChangeColor;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    // This method is triggered when interacting with the object.
    public void OnInteract()
    {
        switch (interactionType)
        {
            case InteractionType.ChangeColor:
                ChangeColor();
                break;
            case InteractionType.Move:
                MoveObject();
                break;
            case InteractionType.Scale:
                ScaleObject();
                break;
        }
    }

    private void ChangeColor()
    {
        objectRenderer.material.color = Random.ColorHSV();
    }

    private void MoveObject()
    {
        transform.position += Vector3.up * 0.5f; // Move the object up when interacted with.
    }

    private void ScaleObject()
    {
        transform.localScale += Vector3.one * 0.1f; // Scale up the object when interacted with.
    }
}