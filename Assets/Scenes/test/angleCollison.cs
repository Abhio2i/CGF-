using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angleCollison : MonoBehaviour
{
    public Transform point1;
    public float angle1;
    public Transform point2;
    public float angle2;
    public Transform IntersectinPoint;
    void Start()
    {
        FindIntersectionPoint();
    }
    private void Update()
    {
        FindIntersectionPoint();
    }

    void FindIntersectionPoint()
    {
        angle1 = point1.eulerAngles.y;
        angle2 = point2.eulerAngles.y;
        float m1 = Mathf.Tan(Mathf.Deg2Rad * angle1);
        float m2 = Mathf.Tan(Mathf.Deg2Rad * angle2);

        float x = (m1 * point1.position.x - m2 * point2.position.x) / (m1 - m2);
        float y = m1 * (x - point1.position.x) + point1.position.z;
        IntersectinPoint.position = new Vector3(x, 31.16f, y);
        Debug.Log("Intersection Point: (" + x + ", " + y + ")");
    }
}


