using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidingScript : MonoBehaviour
{
    [SerializeField]GameObject sameObject;
    float zPos;
    private void OnTriggerEnter(Collider other)
    {
        IAvoid avoid = gameObject.transform.parent.GetComponent<IAvoid>();
        if (transform.parent.gameObject.CompareTag(other.gameObject.tag))
        {
            zPos = gameObject.transform.position.z - other.gameObject.transform.position.z;
            if (avoid != null && sameObject == null && zPos<0f)
            {
                try
                {
                    if (other.transform.parent.gameObject == null)
                        sameObject = other.transform.gameObject;
                    else
                        sameObject = other.transform.parent.gameObject;
                }
                catch
                {

                }
                    
                avoid.AvoidObject(sameObject);
                return;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IAvoid avoid=gameObject.transform.parent.GetComponent<IAvoid>();
        if(other.CompareTag(this.gameObject.tag))
        {
            if(sameObject != null)
            {
                avoid.StopAvoidingObject();
                sameObject=null;
                return;
            }
        }
    }

    private void Initial()
    {
        sameObject = null;
    }
}
