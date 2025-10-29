using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class MissionInfo

{
    public enum Scenario
    {
        ATA,
        ATG,
        ATS
    }

    [SerializeField] 
    public Scenario scenario;
    [SerializeField]
    public string Name;
    [SerializeField]
    public string FormationA;
    [SerializeField]
    public string FormationB;

    public MissionInfo(MissionInfo data = null)
    {
        if (data != null)
        {
            // Copy Position data
            scenario = data.scenario;
            Name = data.Name;
            FormationA = data.FormationA;
            FormationB = data.FormationB;
            
        }
    }

}
