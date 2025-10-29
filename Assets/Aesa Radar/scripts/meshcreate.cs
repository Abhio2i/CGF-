using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class meshcreate : MonoBehaviour
{

    public float Range = 0.5f;
    public float azimuth = 30f;
    public float bar = 30f;
    public UttamRadar uttam;
    private float az = 0f;
    private float br = 0f;
    private float rg = 0f;
    Mesh mesh;
    MeshCollider meshCollider;
    public Vector3[] ver;
    public List<List<int>> item = new List<List<int>>();
    void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        ver =mesh.vertices;
        similarver();
       
        Invoke("updateRadar", 2f);
    }
    private void FixedUpdate()
    {
        if(rg!=Range || azimuth!=az || bar != br)
        {
            rg = Range;
            az = azimuth;
            br = bar;
            updateRadar();
            //Debug.Log("update");
        }
    }
    public void similarver()
    {
        List<Vector3> s=new List<Vector3>();
        for (int i = 0; i < ver.Length; i++)
        {
            if (!s.Contains(ver[i]))
            {
                s.Add(ver[i]);
            }
        }

        for (int i = 0; i < s.Count; i++)
        {
            bool f= false;
            for(int v = 0; v < ver.Length; v++)
            {
                if (ver[v] == ver[i])
                {
                    if (!f)
                    {
                        f = true;
                        item.Add(new List<int>());
                    }
                    item[item.Count-1].Add(v);
                }
            }
        }
    }

    private void upvertex(int index,Vector3 v)
    {
        var d = item[index];
        for(int i = 0; i < d.Count; i++)
        {
            ver[d[i]] = v;
        }
    }

    private Vector3 getvertex(int index)
    {
        var d = item[index][0];
        return ver[d];
    }




    public void updateRadar()
    {
        if (!meshCollider)
            return;
        meshCollider.enabled = false;
        // 0 front right down index 
        // 1 front left down index
        // 2 front right up index
        // 3 front left up index
        var x = Range * Mathf.Tan(Mathf.Deg2Rad * azimuth);
        var y = Range * Mathf.Tan(Mathf.Deg2Rad * bar);
        upvertex(0, new Vector3(0.5f + x, -0.5f - y, Range - 0.5f));
        upvertex(1, new Vector3(-0.5f - x, -0.5f - y, Range - 0.5f));
        upvertex(2, new Vector3(0.5f + x, 0.5f + y, Range - 0.5f));
        upvertex(3, new Vector3(-0.5f - x, 0.5f + y, Range - 0.5f));
        mesh.vertices = ver;
        //mesh.Optimize();
        meshCollider.convex = true;
        meshCollider.enabled = true;
        uttam.SightObjects.Clear();
        uttam.DetectedObjects.Clear();
        uttam.FoundedObjectsRB.Clear();
        uttam.FoundedObjects.Clear();
        
        //Invoke("UpdateObject",0.5f);
    }

    //public void UpdateObject()
    //{
    //    List<GameObject> o = new List<GameObject>(uttam.SightObjects);
    //    foreach(var v in o)
    //    {
    //        if (!meshCollider.bounds.Contains(v.transform.position))
    //        {
    //            uttam.TriggerExit(v.GetComponent<Collider>());
    //            Debug.Log("ex: "+v.name);
    //        }
    //    }
    //}

}
