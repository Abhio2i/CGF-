using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileNavigation : MonoBehaviour
{

    public enum mode
    {
        None,
        ActiveRadar,
        Infrared,
        DataLink
    }

    public enum RangeType
    {
        BVR,
        CCM
    }
    public enum Type
    {
        ATA,
        ATS,
        STA
    }

    public mode NavigationType = mode.None;
    public RangeType missileType = RangeType.BVR;
    public Type type = Type.ATA;
    public GameObject FiredBy;
    public Transform GivenTarget;
    private missileEngine engine;
    
    public GameObject target;
    public bool hit = false;
    public bool Finish  = true;
    public int endCourse = 1000;

    public List<GameObject> objt;
    private void Start()
    {
        engine = GetComponent<missileEngine>();
    }

    private void FixedUpdate()
    {
        if(NavigationType == mode.None)
        {   
            engine.Target = (transform.forward*100f)+transform.position;
        }
        else if (NavigationType == mode.Infrared)
        {
            var tem = 0f;
            GameObject ob = null;
            foreach (var obj in objt)
            {
                if (obj != null && obj.tag.Contains("Flares"))
                {
                    var sp = obj.GetComponent<Specification>();
                    if (sp != null)
                    {
                        if (sp.Temprature > tem)
                        {
                            tem = sp.Temprature;
                            ob = obj;
                        }

                    }
                }
            }
            if (ob)
            {
                var dist = Vector3.Distance(ob.transform.position, transform.position);
                engine.Target = ob.transform.position + ob.transform.forward * dist / 30 ;
            }
            else
            {
                if (objt.Contains(GivenTarget.gameObject))
                {
                    var dist = Vector3.Distance(GivenTarget.transform.position, transform.position);
                    engine.Target = GivenTarget.transform.position + GivenTarget.transform.forward * dist / 30;
                }
                else
                engine.Target = (transform.forward * 100f) + transform.position;
            }
            
        }
        else if (NavigationType == mode.ActiveRadar)
        {
            var chaff = false;
            var max = 0f;
            GameObject loc = null;
            foreach (var i in objt)
            {
                if (i!=null && i.tag == "chaff")
                {
                    chaff = true;
                    float d = Vector3.Distance(transform.position, i.transform.position);
                    if (d < max || max == 0f)
                    {
                        max = d;
                        loc = i;
                    }
                }
            }
            if (chaff && Random.Range(0,10)>3)
            {
                engine.Target = loc.transform.position;
            }
            else
            {
                GameObject ob = null;
                if ((GivenTarget != null && objt.Contains(GivenTarget.gameObject)))
                {
                    ob = GivenTarget.gameObject;
                }

                if (ob)
                {
                    var dist = Vector3.Distance(ob.transform.position, transform.position);
                    engine.Target = ob.transform.position + ob.transform.forward * dist / 30;
                    target = ob;
                }
                else
                {
                    engine.Target = (transform.forward * 100f) + transform.position;
                }
            }
        }
        else if (NavigationType == mode.DataLink)
        {
            target = GivenTarget.gameObject;
            var dist = Vector3.Distance(target.transform.position, transform.position);
            if (target == null) { NavigationType = mode.None; }
            if (dist < endCourse) { NavigationType = mode.ActiveRadar; }
            engine.Target = target.transform.position + target.transform.forward*dist/30;
        }
    }

    public void onenter(Collider other)
    {
        if (!objt.Contains(other.gameObject)&& other.name !="radar")
        {
            objt.Add(other.gameObject);
        }
    }

    public void onstay(Collider other)
    {

    }

    public void onexit(Collider other)
    {
        if (objt.Contains(other.gameObject))
        {
            if (target != null&&other.gameObject==target.gameObject)
            {
                target = null;
                if (!hit)
                {
                   //some 
                }
                
            }
            objt.Remove(other.gameObject);
        }
    }

   
}
