#region Script Info
//inp: positions of Flares to be Spawn.
//out: While fire the flares spawn on that given spot.
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
//using PlaneFunctions;

namespace EW.Flare
{
    public class Flares : MonoBehaviour
    {
        #region Global Variables
        public GameObject flare;
        public GameObject[] flarePositions;
        public Transform[] flareAdd;
        public GameObject missile;
        public EWRadar EW;
        #endregion

        #region Local Variables
        private FlareActive avtiveFlare;
        private int maxFlare = 20;
        private PlayerControle flareTrigger;
        //private BasicPlaneActions basicPlane;
        #endregion

        #region Global Functions
        private void OnEnable()
        {
            flareTrigger.Enable();
        }
        private void OnDisable()
        {
            flareTrigger.Disable();
        }
        private void Awake()
        {
            flareTrigger = new PlayerControle();
            flareTrigger.CounterMeasure.Flare.performed += ctx => EW.SpawnFlare();
            flareTrigger.CounterMeasure.Chaff.performed += ctx => EW.SpawnChaff();
        }
        #endregion
        /*public void SpawnFlare()
        {
            StartCoroutine(DelayedFlaresShoot());
        }

        IEnumerator DelayedFlaresShoot()
        {
            if (maxFlare > 0)
            {
                int length = flarePositions.Length * 4;
                flareAdd = new Transform[length];

                int i = 0; int j = 0;

                for (int k = 0; k < flarePositions.Length;)
                {
                    var _flare = Instantiate(flare);
                    flareAdd[j] = _flare.transform;
                    flareAdd[j].transform.SetPositionAndRotation(flarePositions[i].transform.position, flarePositions[i].transform.rotation);
                    avtiveFlare = flareAdd[j].gameObject.AddComponent<FlareActive>();
                    flareAdd[j].transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.1f);
                    j++;
                    i++;
                    if (i == 4)
                    {
                        i = 0;
                    }
                    if (j % 4 == 0)
                    {
                        k++;
                        //GiveTargetsToMissile();
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                maxFlare--;
            }
        }

        void GiveTargetsToMissile()
        {
            if (missile == null) return;
            missile.GetComponent<TargetFollow>().FlaresActivated(flareAdd);
        }*/
    }

}