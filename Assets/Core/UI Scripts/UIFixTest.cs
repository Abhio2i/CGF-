using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFixTest : MonoBehaviour
{
    public bool fix = false;
    void Start()
    {
        
    }

    private void OnValidate()
    {
        if (fix)
        {
            fix = false;
            RectTransform[] allObjects = GameObject.FindObjectsOfType<RectTransform>();
            // Use LINQ to filter objects by name
            var objectsWithName = allObjects.Where(obj => obj.gameObject.name == "Image").ToArray();

            // Print the found objects to the console
            foreach (var obj in objectsWithName)
            {
                if(obj.sizeDelta.y == 0.6f)
                {
                    obj.sizeDelta = new Vector2(obj.sizeDelta.x, 1f);
                }
            }
        }
    }
}
