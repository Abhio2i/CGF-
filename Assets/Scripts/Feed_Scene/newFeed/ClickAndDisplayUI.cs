using Assets.Scripts.Feed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class ClickAndDisplayUI : MonoBehaviour
    {
        public static Camera camera;

        private Ray ray;
        private RaycastHit hit;

        void Click()
        {
            if(Input.GetMouseButtonDown(0))
            {
                ray = new Ray(camera.ScreenToWorldPoint(Input.mousePosition), camera.transform.forward);
                if(Physics.Raycast(ray, out hit,1000f))
                {
                    if(hit.collider.gameObject.GetComponent<SetEntityPositionAndRotation>()!=null)
                    {

                    }
                }
            }
        }
    }
}