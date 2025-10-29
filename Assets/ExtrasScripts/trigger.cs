using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{

    public missileNavigation navig; 

    private void OnTriggerEnter(Collider other)
    {
        navig.onenter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        navig.onstay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        navig.onexit(other);
    }
}
