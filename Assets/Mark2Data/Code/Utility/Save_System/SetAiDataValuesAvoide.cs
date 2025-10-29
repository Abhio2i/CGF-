using System.Collections;
using System.Collections.Generic;
//using Unity.Barracuda;
//using Unity.MLAgents;
//using Unity.MLAgents.Actuators;
//using Unity.MLAgents.Policies;
//using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Code.Utility.Save_System
{
    public class SetAiDataValuesAvoide //: Agent
    {
        [HideInInspector]
        public GameObject otherPlanPos;
        //public NNModel neuralBrain;
        //private BehaviorParameters currentBrain;
        public List<Vector3> track;
        public List<Vector3> actualTrack;
        public int max, min;
        public int pos;

        public float delta, rotSpeed, maxSpeed, resetSesson, RewardVal, ChaseReward;

        public bool inRange, isScanning, isLap;

        private int total_waypoints;

        private bool isNotCompleteLap;
        private bool isAvoide;

        Vector3 targetPos;
        Quaternion rotation;
        void Start()
        {
            actualTrack = new List<Vector3>();
            isScanning = false;
            total_waypoints = max;
            //transform.localPosition = RandomPos();
            isScanning = true;
        }
        //public override void CollectObservations(VectorSensor sensor)
        //{
        //    sensor.AddObservation(transform.localPosition);
        //    sensor.AddObservation(transform.localRotation);
        //}
        //public override void OnEpisodeBegin()
        //{
        //    isScanning = false;

        //    CancelInvoke();
        //    StopAllCoroutines();
        //    isNotCompleteLap = isLap = false;
        //    isAvoide = true;
        //    pos = min; RewardVal = 0;
        //    Invoke(nameof(RestartSession), resetSesson);
        //    isScanning = true;

        //}

        private void RestartSession()
        {
            isNotCompleteLap = isLap = true;
        }
        //private void EndSession()
        //{
        //    EndEpisode();
        //    //transform.localPosition = RandomPos();
        //}

        //private void FixedUpdate()
        //{
        //    if (!isScanning) { return; }
        //    //if (Vector3.Distance(track[pos], transform.localPosition) < delta && otherPlanPos == null)
        //    {
        //        pos++;
        //        if (pos >= total_waypoints)
        //        {
        //            pos = min;
        //            //AddReward(5f);
        //            RewardVal += 5f;
        //            OnEposideEnd();
        //        }
        //    }
        //    if (inRange)
        //    {
        //        AddReward(ChaseReward);
        //    }
        //    if (isNotCompleteLap)
        //    {
        //        AddReward(-Time.deltaTime);
        //        RewardVal -= Time.deltaTime;
        //        if (isLap)
        //        {
        //            isLap = false;
        //            Invoke(nameof(EndSession), 30f);
        //        }
        //    }

        //}

        //    public override void Heuristic(in ActionBuffers actionsOut)
        //    {
        //        ActionSegment<float> continousInput = actionsOut.ContinuousActions;
        //        ActionSegment<int> discreteInput = actionsOut.DiscreteActions;

        //        continousInput[0] = Input.GetAxis("Horizontal");
        //        discreteInput[0] = (int)Input.GetAxisRaw("Vertical");
        //    }

        //    public override void OnActionReceived(ActionBuffers actions)
        //    {
        //        if (!isScanning) { return; }

        //        if (otherPlanPos != null)
        //        {
        //            targetPos = otherPlanPos.transform.position;
        //        }
        //        else
        //        {
        //            targetPos =track[pos];
        //        }
        //        Vector3 relativePos = transform.localPosition - targetPos;
        //        if (otherPlanPos != null)
        //        {
        //            rotation = Quaternion.LookRotation(-relativePos, Vector3.up);
        //            isAvoide = false;
        //            Invoke(nameof(ExitAvoid), 3f);
        //        }
        //        else if (isAvoide)
        //        {
        //            rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //        }
        //        rotation.x += actions.DiscreteActions[0];
        //        Vector3 move = Mathf.Abs(actions.ContinuousActions[0]) * maxSpeed * transform.forward;

        //        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotSpeed * Time.deltaTime);
        //        transform.localPosition -= Time.deltaTime * move;
        //    }

        //    private void ExitAvoid()
        //    {
        //        isAvoide = true;
        //    }

        //    private void OnTriggerEnter(Collider other)
        //    {
        //        if (!isScanning) { return; }
        //        if (other.CompareTag("Ground"))
        //        {
        //            AddReward(-1f);
        //            RewardVal -= 1f;
        //            OnEposideEnd();
        //        }
        //        if (other.CompareTag("PlayerBody"))
        //        {
        //            AddReward(-2f);
        //            RewardVal -= 2f;
        //            OnEposideEnd();
        //        }
        //    }
        //    void OnEposideEnd()
        //    {
        //        EndEpisode();
        //        //transform.localPosition = RandomPos();
        //    }

        //    Vector3 RandomPos()
        //    {
        //        float x = Random.Range(-40000f, 40000f);
        //        return new Vector3(x, 1000, x);
        //    }
        //}
    }
}