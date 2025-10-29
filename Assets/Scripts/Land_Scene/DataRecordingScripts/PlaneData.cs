using Assets.Scripts.Feed_Scene.newFeed;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Feed
{
    public class PlaneData : MonoBehaviour
    {
        [SerializeField] MapLatLong planLatLon;

        private SaveEntityDatas saveAllData;
        public Specification specification;
        private Rigidbody rigidbody;
        private float _prevG = 1;


        public float g_force=0;
        public float speed;
        
        public int missileCount;
        public int gunCount;
        
        public string message="";

        public Vector2 latLong;

        public Transform planeTransform;

        public bool trigger1,trigger2;
        private void OnEnable()
        {
            specification = GetComponent<Specification>();
            saveAllData = GetComponent<SaveEntityDatas>();
            planLatLon = FindObjectOfType<MapLatLong>();
        }
        private bool isNewMessage;
        public void SetMessage(string newMessage)
        {
            message = newMessage;
        }
        public string Message()
        {
            return message;
        }
        private void FixedUpdate()
        {
            //g_force = Random.Range(1, 10);
            Vector3 localVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
            Vector3 localAngularVelocity = transform.InverseTransformDirection(rigidbody.angularVelocity);
            float turnRadius = (Mathf.Approximately(localAngularVelocity.x, 0.0f)) ? float.MaxValue : localVelocity.z / localAngularVelocity.x;
            float turnForce = (Mathf.Approximately(turnRadius, 0.0f)) ? 0.0f : (localVelocity.z * localVelocity.z) / turnRadius;
            float baseG = turnForce / -9.81f; baseG += transform.up.y * (Physics.gravity.y / -9.81f);
            float targetG = (baseG * 0.04f) + (_prevG * (1.0f - 0.04f)); _prevG = targetG;
            g_force = (float)Math.Round(targetG, 1);
            if(specification)
                speed = specification.entityInfo.Speed;
            latLong = planLatLon.CalculateLatLong(planeTransform.position.z, planeTransform.position.x);
            if (planeTransform != null)
            {
                saveAllData.SetMessage(message);
            }
            message = "";
        }
        private void Awake()
        {
            planeTransform = this.transform;
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}
