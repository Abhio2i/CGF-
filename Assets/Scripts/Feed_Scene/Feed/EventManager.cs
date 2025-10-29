using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Feed
{
    public class EventManager : MonoBehaviour
    {
        public List<Transform> eventsMessages = new List<Transform>();
        public void Active(bool active)
        {
            //if (CameraManager.selected == this.gameObject)
            //{
            //    if (eventsMessages.Count <= 0)
            //        FeedSceneManager.scrollView.SetActive(false);
            //    else
            //        FeedSceneManager.scrollView.SetActive(true);
            //}

            foreach (Transform t in eventsMessages)
            {
                t.gameObject.SetActive(active);
            }
        }
    }
}