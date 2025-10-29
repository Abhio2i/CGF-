using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum aircraftType
{
    Ally,Enemy,Neutral,Player
}

public class HEAT : MonoBehaviour
{
    public aircraftType aircraftType;
    public float tempertaure;
}
