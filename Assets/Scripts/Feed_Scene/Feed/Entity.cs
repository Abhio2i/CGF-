using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Feed
{
    [CreateAssetMenu(fileName = "Entity Data")]
    public class Entity : ScriptableObject
    {
        public List<Vector3> positions;
        public List<Vector3> rotations;
        public List<float> gforce;
        public List<float> missileCount;
        public List<string> eventType;
        public List<string> time;
        public string type;
    }
}