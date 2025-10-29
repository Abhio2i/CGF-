using UnityEngine;
using UnityEngine.UI;

public class SquareLineRendererOnCanvas : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component attached to the Panel.
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure the LineRenderer component is valid.
        if (lineRenderer != null)
        {
            // Define the vertices of the square.
            Vector3[] squareVertices = new Vector3[5];

            // Set the coordinates of the square's vertices.
            float size = 100.0f; // Change this value to adjust the size of the square.

            squareVertices[0] = new Vector3(-size, -size, 0);
            squareVertices[1] = new Vector3(size, -size, 0);
            squareVertices[2] = new Vector3(size, size, 0);
            squareVertices[3] = new Vector3(-size, size, 0);
            squareVertices[4] = new Vector3(-size, -size, 0); // Closing the square

            // Set the LineRenderer's positions to the square's vertices.
            lineRenderer.positionCount = squareVertices.Length;
            lineRenderer.SetPositions(squareVertices);
        }
        else
        {
            Debug.LogError("LineRenderer component not found.");
        }
    }
}
