using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCulling : MonoBehaviour
{
    public Transform Player;
    public List<Terrain> terrainsLow = new List<Terrain>();
    public int LowRange = 5000;
    public List<Terrain> terrainsHigh = new List<Terrain>();
    public int HighRange = 5000;
    // Start is called before the first frame update
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

        foreach (Terrain terrain in terrainsLow)
        {
            Vector3 t = terrain.transform.position;
            //t.y = 0;
            Vector3 dir = t - p;
            float angle = Vector3.Angle(dir, f);

            float distance = Vector3.Distance(t, p);
            if (distance < LowRange )
            {
                terrain.enabled = true;
            }
            else
            {
                terrain.enabled = false;
            }
        }
    }
}
