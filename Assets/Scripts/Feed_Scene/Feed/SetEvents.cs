using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Assets.Scripts.Feed
{
    public class SetEvents : MonoBehaviour
    {
        public List<string> events = new List<string>();

        public List<string> time = new List<string>();

        public List<Transform> textObjects= new List<Transform>();  

        public List<int> eventsIndex = new List<int>();

        public List<GameObject> eventSymbols=new List<GameObject>();

        public List<Vector3> eventPositions = new List<Vector3>();

        public GameObject eventSymbol,parent;

        public Emulator emulator;
        public bool IAmactive = false;

        public int maxFrames;
        
        public void CreateEventIcons()
        {
            for(int i = 0; i < events.Count; i++)
            {
                if (events[i].Length!=0 && events[i]!="")
                {
                    GameObject icon = Instantiate(eventSymbol);

                    icon.transform.SetParent(parent.transform,false);

                    float offset = (float)i / maxFrames;

                    icon.GetComponent<RectTransform>().anchorMin = new Vector2(offset,0);
                    icon.GetComponent<RectTransform>().anchorMax = new Vector2(offset,1);
                    
                    eventSymbols.Add(icon);
                    eventPositions.Add(new Vector2(offset, 0));
                    eventsIndex.Add(i);
                }
            }
            EnableDisable(false);
        }

        public void EnableDisable(bool active)
        {
            IAmactive = active;
            foreach(GameObject go in eventSymbols)
            {
                go.SetActive(active);
            }
        }

        public void CreateTextObjects(GameObject textObject,Transform parent)
        {

            for(int i=0;i<events.Count;i++)
            {
                var _string=events[i];
                var _time = time[i];

                if (_string != "")
                {
                    var _object = Instantiate(textObject);
                    TMP_Text message = _object.transform.GetChild(0).GetComponent<TMP_Text>();
                    TMP_Text time = _object.transform.GetChild(1).GetComponent<TMP_Text>();
                    message.text = _string;
                    time.text = _time;
                    _object.transform.SetParent(parent, false);
                    textObjects.Add(_object.transform);
                }
            }
            gameObject.AddComponent<EventManager>().eventsMessages=textObjects;
        }
    }
}