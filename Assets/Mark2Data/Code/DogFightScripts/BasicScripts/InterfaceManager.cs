using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace interfaces
{
    public interface IBasicPlaneFunctions
    {
        public void Formation(Rigidbody plane, Vector3 target, float speed, float rotateSpeed); public void Run(float speed, Vector3 targetPos, Rigidbody plane, float rotateSpeed);
        public void Chase(Rigidbody plane, Transform target, float speed, float rotateSpeed);
        public void Patrol(float speed, Vector3 currentPos, Rigidbody plane, float rotateSpeed);
        public void Fire(GameObject _object, Vector3 pos, float time, Transform target, GameObject parent, GameObject explosion);
        public void Avoid(float speed, Rigidbody plane, Quaternion target, float rotateSpeed);
    }
    public interface IFormation
    {
        public void FormationActive(bool active);
        public void SelectLeader();
        public void SelectSquad();
        public void FollowLeader(Vector3 pos);
    }
    public interface I_Chase
    {
        public void Chase(Transform target);
    }
    public interface I_PlayerDestroyed
    { public void PlayerDestroyed(); 
    }
    public interface IAvoidGround
    { 
        public void AvoidGround();
    }
    public interface ICallingSquadMembers
    {
        public void CallingSquadMembers(bool active,Transform target);
    }
    public interface I_Run
    {
        public void Run();
    }

    public interface I_Avoid
    {
        public void Avoid();

    }

    public interface I_Fire
    {
        public void Fire(Transform target);
    }
    public interface I_Damage
    {
        public void Damage();
    }

}