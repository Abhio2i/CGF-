using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCulling : MonoBehaviour
{
    public Transform Player;
    public Transform testObject;
    public float Angle = 0;
    public List<Terrain> terrains = new List<Terrain>();
    public int Range = 5000;
    public int LowRange = 5000;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = Player.position;
        //p.y = 0;
        Vector3 f = Player.forward;
        //Vector3 to = testObject.position;
       // Vector3 dir = to - p;
        //Angle = Vector3.Angle(dir,Player.forward);

        foreach (Terrain terrain in terrains)
        {
            Vector3 t = terrain.transform.position;
            //t.y = 0;
            Vector3 dir = t - p;
            float angle = Vector3.Angle(dir,f);

            float distance = Vector3.Distance(t,p);
            if (distance < Range&&angle<Angle)
            {
                terrain.enabled = true;
            }
            else
            {
                if(distance < LowRange)
                {
                    if (angle < 90||distance <5000)
                    {
                        terrain.enabled = true;
                    }
                    else
                    {
                        terrain.enabled = false;
                    }
                    
                }
                else
                terrain.enabled = false;
            }
        }
    }
}
