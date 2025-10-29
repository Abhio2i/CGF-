using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Assets.Scripts.Feed_Scene.newFeed;
namespace Assets.Scripts.Feed
{
    public class SetEntityPositionAndRotation : MonoBehaviour
    {
       
        private List<Vector3> _position = new List<Vector3>();
        private List<Quaternion> _rotation = new List<Quaternion>();
        
        private List<Vector2> _LatLong = new List<Vector2>();

        public List<string> str_positions, str_rotation, latLong = new List<string>();
        public List<float> _gforce = new List<float>();
        public bool iAmActive;
        public EntityUI_Specs specs;
        public int firstframe,lastframe;
        public Material ActiveMaterial, DestroyMaterial;
        public NewFeedSceneManager manager;
        public Vector3 currentPos,latitudeLong;
        public Quaternion currentRot;
        public float gforce;
        public bool missile;

        private Transform npc;

        private int frameNumber;
        private bool activate;
        private LineRenderer lineRenderer;
        private Vector3[] position;

        void Convert(List<string> fromFormat,List<Vector3> toFormat)
        {
            string[] temp= new string[3];
            for(int i = 0; i < fromFormat.Count; i++)
            {
                temp = fromFormat[i].Split(',');

                float x = float.Parse(temp[0]);
                float y = float.Parse(temp[1]);
                float z = float.Parse(temp[2]);

                toFormat.Add(new Vector3(x, y, z));
            }
        }
        void Convert(List<string> fromFormat,List<Quaternion> toFormat)
        {
            string[] temp = new string[4];
            for (int i = 0; i < fromFormat.Count; i++)
            {
                temp = fromFormat[i].Split(',');

                float x = float.Parse(temp[0]);
                float y = float.Parse(temp[1]);
                float z = float.Parse(temp[2]);
                float w = float.Parse(temp[3]);

                toFormat.Add(new Quaternion(x, y, z,w));
            }
        }
        void Convert(List<string> fromFormat, List<Vector2> toFormat)
        {
            string[] temp = new string[2];
            for (int i = 0; i < fromFormat.Count; i++)
            {
                temp = fromFormat[i].Split(',');

                float x = float.Parse(temp[0]);
                float y = float.Parse(temp[1]);

                toFormat.Add(new Vector2(x, y));
            }
        }
        void ListToArray()
        {
            position = new Vector3[str_positions.Count];
            for(int i=0;i<position.Length;i++)
            {
                Vector3 temp = _position[i];
                temp.y = temp.y - 10f;
                position[i] = temp;
            }
        }
        public void Initialize()
        {
            Convert(str_positions, _position);
            Convert(str_rotation,_rotation);
            Convert(latLong, _LatLong);
            
            npc = this.transform;
            npc.transform.position = _position[0];
            specs=GetComponent<EntityUI_Specs>();

            ListToArray();

            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 5f;
            //lineRenderer.material = DestroyMaterial;
            //lineRenderer.SetColors(Color.white,Color.)
            lineRenderer.positionCount = position.Length;
            lineRenderer.SetPositions(position);
            lineRenderer.enabled = true;
        
            

            activate = true;
        }
        bool alreadyChanged;
        [SerializeField]int frame;
        private void FixedUpdate()
        {
            if (!activate) return;

            //frameNumber=PlayerPrefs.GetInt("CurrentFrame");
            frameNumber = Emulator.currentFrame;
            frame = frameNumber;
            if (!missile)
            {
                if (frameNumber >= lastframe && !alreadyChanged)
                {
                    manager.ChangeChildrenColorAccordingToType(transform, DestroyMaterial);
                    alreadyChanged = true;
                    return;
                }
                else if (frameNumber < lastframe && alreadyChanged)
                {
                    manager.ChangeChildrenColorAccordingToType(transform, ActiveMaterial);
                    alreadyChanged = false;
                    return;
                }
                frame=frameNumber;
            }
            else
            {
                frame = frameNumber - firstframe;
            }
            try { 
                
                currentPos = _position[frame];
                currentRot = _rotation[frame];
                latitudeLong = _LatLong[frame];
                gforce = _gforce[frame];
                npc.transform.position = currentPos;//Vector3.Lerp(npc.transform.position, currentPos, 0.98f * Time.fixedDeltaTime);
                npc.transform.rotation = currentRot; //Quaternion.Slerp(npc.
                                                     //transform.rotation, currentRot, 0.98f * Time.fixedDeltaTime);
                manager.HideGameObject(npc.transform, true);
                iAmActive=true;
            }
            catch
            {
                manager.HideGameObject(npc.transform, false);
                iAmActive = false;
            }
            

            if (specs == null) return;
            specs.MoveUIObjectWithMainObject();
        }
    }
}