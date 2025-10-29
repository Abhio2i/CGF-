using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIFinder : MonoBehaviour
{
    public TMP_Text planeAprochWarning;
    private int distance;
    private bool isAiFound;
    private GameObject AiBody = null;
    private void Start()
    {
        isAiFound = false;
        planeAprochWarning.text = "";
    }
    private void FixedUpdate()
    {
        if (isAiFound && AiBody != null)
        {
            distance = (int)Vector3.Distance(AiBody.transform.position, transform.position);
            planeAprochWarning.text = "Plane Aproching : " + distance.ToString("0000") + "M";
        }
        else
        {
            planeAprochWarning.text = "";
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.layer == 8)
        {
            isAiFound = true;
            AiBody = other.gameObject;
        }
        else
        {
            isAiFound = false;
            AiBody = null;

        }
    }
}
