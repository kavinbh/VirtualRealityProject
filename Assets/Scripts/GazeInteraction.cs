using UnityEngine;

public class GazeInteraction : MonoBehaviour
{
    private Renderer objectRenderer;
    public Color highlightColor = Color.red;
    private Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    public void OnPointerEnter()
    {
        objectRenderer.material.color = highlightColor;
    }

    public void OnPointerExit()
    {
        objectRenderer.material.color = originalColor;
    }
}