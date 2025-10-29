#region Script Info
//out: Stop the rotation of Plane_Png in z Direction.
#endregion
using UnityEngine;

public class PNGRotation : MonoBehaviour
{
    #region local perimeters
    Quaternion rotation;
    #endregion

    #region local Functions
    void Start()
    {
        rotation = transform.rotation;        
    }

    // Update is called once per frame
    void Update()
    {
        //rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;
    }
    #endregion
}
