#region Script Info
//output: If flare fired it moves the flare to random directions.
#endregion
using System.Collections;
using UnityEngine;

namespace EW.Flare
{
    public class FlareActive : MonoBehaviour
    {
        #region LocalVariables
        //int choice;
        #endregion

        #region LocalFunctions
        void Start()
        {
            //choice = Random.Range(0, 2);
            Invoke(nameof(Disable),20f);
        }
        void Disable()
        {
            gameObject.SetActive(false);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(70, 10, 70));
        }
        void Update()
        {
            if(EWRadar.LeftRightEqualFlares) 
            {
                EWRadar.LeftRightEqualFlares = false;
                transform.position += transform.right * Time.deltaTime * 20f;
            }
            else 
            {
                EWRadar.LeftRightEqualFlares = true;
                transform.position -= transform.right * Time.deltaTime * 20f;
            }
        }
        public IEnumerator destroy()
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
        #endregion
    }
}