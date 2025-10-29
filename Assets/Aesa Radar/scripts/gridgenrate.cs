using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridgenrate : MonoBehaviour
{
    public int size = 2;
    public float scale = 1f;
    public bool genrate = false;
    public List<GameObject> l=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (genrate)
        {
            genrate = false;
            foreach(var ob in l)
            {
                Destroy(ob.gameObject);
            }


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.position = transform.position + new Vector3(i * scale, 0, j * scale);
                    l.Add(obj);
                }
            }
        }
    }
}
