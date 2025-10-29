using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissileInfo
{
    public string Name;
    public string RangeType;
    public string Type;
    public float Speed;
    public float Range;
    public float TurnRadius;
    public int count;

    public MissileInfo(MissileInfo data = null)
    {
        if (data != null)
        {
            Name = data.Name;
            RangeType = data.RangeType;
            Type = data.Type;
            Speed = data.Speed;
            Range = data.Range;
            TurnRadius = data.TurnRadius;
            count = data.count;
}
    }
}
