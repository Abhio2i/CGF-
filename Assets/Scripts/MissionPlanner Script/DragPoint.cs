using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPoint : MonoBehaviour
{
    public Dropdown option;
    public bool isDrag = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnDrag(BaseEventData eventData)
    {
        if (option == null)
        {
            transform.position = Input.mousePosition;
        }
        else
        {
            if(option.value == 0)
            {
                transform.position = Input.mousePosition;
            }
        }

        
    }
}
