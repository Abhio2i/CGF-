using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MAWS_System : MonoBehaviour
{
    [SerializeField] GameObject respondText;
    [SerializeField] GameObject respondLogo;
    public void ActivateDeactivate(bool trigger)
    {
        respondText.gameObject.SetActive(trigger);
        respondLogo.gameObject.SetActive(trigger);
    }
}
