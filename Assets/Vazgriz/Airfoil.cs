using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vazgriz.Plane
{
    public class Airfoil : MonoBehaviour
    {
        public enum AirfoilType
        {
            Horizontal,
            Vertical
        }

        [SerializeField]
        AirfoilType type;
        [SerializeField]
        float sweepAngle;
        [SerializeField]
        float liftPower;
        [SerializeField]
        float inducedDrag;
        [SerializeField]
        Vector3 inputInfluence;
        [SerializeField]
        float inputSpeed;
        [SerializeField]
        float aoaInputRange;
        [SerializeField]
        float trim;

        new Transform transform;
        float input;

        public AirfoilType Type
        {
            get
            {
                return type;
            }
        }

        public float SweepAngle
        {
            get
            {
                return sweepAngle;
            }
        }

        public float LiftPower
        {
            get
            {
                return liftPower;
            }
        }

        public float InducedDrag
        {
            get
            {
                return inducedDrag;
            }
        }

        public Vector3 LocalPosition
        {
            get
            {
                if(transform == null)
                {
                    return Vector3.zero;
                }
                return transform.localPosition;
            }
        }

        public float AOABias
        {
            get
            {
                return input * aoaInputRange + trim;
            }
        }

        void Start()
        {
            transform = GetComponent<Transform>();
        }

        public void SetInput(float dt, Vector3 input)
        {
            var influence = Vector3.Scale(input, inputInfluence);
            this.input = Utilities.MoveTo(this.input, influence.x + influence.y + influence.z, inputSpeed, dt, -1, 1);
        }
    }
}