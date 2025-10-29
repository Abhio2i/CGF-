using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisConverstion : MonoBehaviour
{
    public Vector3 originalPosition;
    public Vector3 NewPosition;
    public Vector3 originalVelocity;
    public Vector3 NewVelocity;
    public Vector3 originalAcceleration;
    public Vector3 NewAcceleration;
    public Rigidbody rb;
    public Specification Specification;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        originalPosition = transform.position;
        NewPosition = new Vector3(originalPosition.z,originalPosition.x,originalPosition.y);

        originalVelocity = new Vector3(Specification.j, Specification.k, Specification.i);
        NewVelocity = new Vector3(Specification.i, Specification.j, Specification.k);

        originalAcceleration = new Vector3(Specification.entityInfo.xAccel, Specification.entityInfo.yAccel, Specification.entityInfo.zAccel);
        NewAcceleration = new Vector3(originalAcceleration.z, originalAcceleration.x, originalAcceleration.y);

    }
}
