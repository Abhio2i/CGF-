using UnityEngine;


public class customEvent : MonoBehaviour
{
    //[SerializeField]
    //public UnityEvent mouseDown;
    
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        // Calculate the offset between the object's position and the mouse position
        offset = transform.position - GetMouseWorldPosition();

        // Set isDragging to true
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse position
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        // Set isDragging to false
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}

