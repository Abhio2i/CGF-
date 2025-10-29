using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class Selector : MonoBehaviour
{
    public List<GameObject> filterObj;
    public GameObject selectedObject;
    public Transform mainPlane;
    public float maxSelectionDistance;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            var dd = new List<GameObject>(filterObj);
            foreach (GameObject obj in dd)
            {
                if (obj == null)
                {
                    filterObj.Remove(obj);
                }
            }
            if (filterObj.Count == 0) return;
            selectedObject = SelectClosest(Input.mousePosition);
            SetLayerForChild(selectedObject, 7,5);
            //Debug.Log(selectedObject);
        }
        if (Input.GetKeyDown(KeyCode.Delete)&& selectedObject!=null)
        {
            Destroy(selectedObject.gameObject);
        }
        if (selectedObject)
        {
            if (Input.GetKey(KeyCode.E))
            {
                selectedObject.transform.Rotate(0, 30 * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                selectedObject.transform.Rotate(0, -30 * Time.deltaTime, 0);
            }
            selectedObject.transform.position += new Vector3(-Input.GetAxis("Vertical"),0,Input.GetAxis("Horizontal"))*Time.deltaTime* 30 * Camera.main.orthographicSize / 100;
        }
        if (Input.GetKeyDown(KeyCode.F) && selectedObject.tag.Contains("Missile"))
        {
            selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            //selectedObject.transform.SetPositionAndRotation(transform.position + (transform.forward * 7f) + (transform.up * -4f), transform.rotation);
            selectedObject.GetComponent<missileEngine>().fire = true;
            if (Vector3.Angle(mainPlane.position - selectedObject.transform.position, selectedObject.transform.forward) < 15)
                selectedObject.GetComponent<missileNavigation>().GivenTarget = mainPlane;
        }
    }
    GameObject SelectClosest(Vector3 pos)
    {
        
        if (!filterObj[0]) { filterObj.RemoveAt(0); }
        SetLayerForChild(filterObj[0], 5,7);
        Vector2 ppos = Camera.main.WorldToScreenPoint(filterObj[0].transform.position);
        var close = Vector3.Distance(pos,ppos);
        int closest = 0;
        for(int i = 1; i < filterObj.Count; i++)
        {
            if (!filterObj[i]) { continue; }
            ppos = Camera.main.WorldToScreenPoint(filterObj[i].transform.position);
            SetLayerForChild(filterObj[i], 5,7); ;
            float x = Vector3.Distance(pos, ppos);
            if (x < close)
            {
                closest = i;
                close = x;
            }
        }
        if(close < maxSelectionDistance)
        return filterObj[closest];
        return null;
    }
    void SetLayerForChild(GameObject obj, int layer,int oldLayer)
    {
        if (obj == null) return;
        foreach(Transform child in obj.transform)
        {
            if(child.gameObject.layer == oldLayer)
            child.gameObject.layer = layer;
            if (child.GetComponentInChildren<Transform>())
            {
                SetLayerForChild(child.gameObject, layer,oldLayer);
            }
        }
    }
}
