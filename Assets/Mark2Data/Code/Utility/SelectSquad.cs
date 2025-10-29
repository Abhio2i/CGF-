using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ally.Group
{
    public class SelectSquad : MonoBehaviour
    {
        public List<GameObject> allyPlanes;
        public bool isCreate;
        public GameObject followThis;
        public List<GameObject> allyPlanesSquad;
        public GameObject myCurrentPlane;
        public int isLock=0;

        private void Awake()
        {
            allyPlanes = new List<GameObject>();
           
            //allyPlanes.Clear();
        }
        bool passOnce = true;
        private void FixedUpdate()
        {
           
        }
        public void CreateSquad()
        {
           
            //select squad of 3
            
            if (isCreate)
            {
                allyPlanes.Remove(myCurrentPlane);
                //Debug.Log("here");
                for (int i = 0; i < 2; i++)
                {
                    if (allyPlanes.Count>0)
                    {
                        GameObject a=allyPlanes[i];
                        //a.GetComponent<AllyStateHandling>().state = allyState.Chase;
                        //a.GetComponent<AllyStateHandling>().enemy = followThis;
                        allyPlanesSquad.Add(a);
                        allyPlanes.RemoveAt(i); 
                    }
                }
            }
            if(!isCreate && allyPlanesSquad!=null)
            {
                for(int i=0;i<allyPlanesSquad.Count;i++)
                {
                    allyPlanes.Add(allyPlanesSquad[i]);
                    //if (allyPlanesSquad[i].GetComponent<AttackScript>() == null)
                    //{
                    //    allyPlanesSquad[i].GetComponent<AllyStateHandling>().state = allyState.Follow;
                    //    allyPlanesSquad[i].GetComponent<AllyStateHandling>().enemy = null;
                    //}
                }
                allyPlanesSquad.Clear();
                if (myCurrentPlane != null)
                {
                    allyPlanes.Add(myCurrentPlane);
                }
                myCurrentPlane = null;
                passOnce = !passOnce;
            }
        }
    }
}