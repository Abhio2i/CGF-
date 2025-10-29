#region Script Info
//input: plane radar position, direction , speed etc.
//output : A functional radar is get integrated in the scene and the radar is start working and seen in UI.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data.Plane;
using UI.PingRespon;
namespace AirPlane.Radar
{
    public class Radar : MonoBehaviour
    {
        #region ControlParameters

        [Header("Ping")]
        public bool pingColor = false;
        public Image pingImg;
        public Sprite Img;
        public GameObject canvasMain;
        public GameObject canvasCockpit;
        public GameObject RadarObj;
        public float speed;

        [Header("Radar_Perimeters")]
        public int distance = 20;
        public int angle = 30;

        public Color meshColor = Color.white;
        public LayerMask layers;
        public LayerMask collisionLayer;
        public List<GameObject> Enemys
        {
            get
            {
                enemys.RemoveAll(obj => !obj);
                return enemys;
            }
        }

        readonly List<GameObject> enemys = new List<GameObject>();
        readonly Collider[] colliders = new Collider[100];

        [SerializeField] private PlaneData planeData;

        Mesh mesh;
        int count;
        private int mile = 1600; // 1 mile = 1600 meters;
        private bool isScan = true;
        [HideInInspector] public int angleUPDown = 40;
        [HideInInspector] public int bar = 40;
        private GameObject PlaneBody;

        private bool IFF = false;
        #endregion

        #region UnityFunctions
        private void Start()
        {
            PlaneBody = planeData.body;
            angleUPDown = bar = 40;
            distance *= mile;
        }
        private void Update()
        {
            if(RadarObj != null)
            {
                MoveUpDown();
            }
        }
        private void FixedUpdate()
        {
            if (!isScan | RadarObj == null) return;
            StartCoroutine(Scan());
        }
        private void OnValidate()
        {
            mesh = RadarDraw();
        }

        private void OnGizmos()
        {
            if (mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, RadarObj.transform.position, RadarObj.transform.rotation);
            }
            Gizmos.color = Color.green;
            foreach (var obj in enemys)
            {
                Gizmos.DrawSphere(obj.transform.position, 2f);
            }
        }
        #endregion

        #region Global Functions
        public bool IsInSight(GameObject obj, UnityEngine.Vector3 direction)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (direction.y < -bar || direction.y > bar)
            {
                return false;
            }
            direction.y = 0;
            float deltaAngle = UnityEngine.Vector3.Angle(direction, RadarObj.transform.forward);
            if (deltaAngle > angle)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Local Function
        private IEnumerator Scan()
        {
            isScan = false;
            count = Physics.OverlapSphereNonAlloc(RadarObj.transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
            enemys.Clear();
            print(count);
            for (int i = 0; i < count; ++i)
            {
                GameObject obj = colliders[i].gameObject;
                UnityEngine.Vector3 dis = obj.transform.position - RadarObj.transform.position;
                I_CallSign callSigh = obj.GetComponent<I_CallSign>();
                if (IsInSight(obj, dis))
                {
                    if(callSigh != null) 
                    {
                        string iffcode = PlayerPrefs.GetString("IFFCode");
                        IFF = callSigh.IFFCode_Checker(iffcode);
                        print(iffcode + "iffcode");
                    }
                    enemys.Add(obj);
                    SpawnPing(canvasMain,obj, dis, IFF);
                    SpawnPing(canvasCockpit,obj, dis, IFF);
       
                }
            }
            yield return new WaitForSeconds(.1f);
            isScan = true;

        }

        private void SpawnPing(GameObject canvas,GameObject obj, UnityEngine.Vector3 dis ,bool iff)
        {

            Image ping = Instantiate(pingImg);
            ping.transform.SetParent(canvas.transform, false);
            ping.GetComponent<RectTransform>().sizeDelta = pingImg.GetComponent<RectTransform>().sizeDelta;
            ping.GetComponent<RectTransform>().position = pingImg.GetComponent<RectTransform>().position;
            ping.GetComponent<RectTransform>().localScale = pingImg.GetComponent<RectTransform>().localScale;

            PingRespond pingScript = ping.GetComponent<PingRespond>();
            pingScript.pingImg.sprite = Img;
            pingScript.target = obj.transform;
            pingScript.radarPos = RadarObj.transform;
            pingScript.pos = dis;
            pingScript.isIFFCheck = pingColor;
            pingScript.IFFCode = iff;
            ping.gameObject.SetActive(true);

        }



        Mesh RadarDraw()
        {
            Mesh mesh = new Mesh();

            int segment = 10;
            int numTriangles = (segment * 4) + 2 + 2;
            int numVertices = numTriangles * 3;

            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[numVertices];
            int[] triangles = new int[numVertices];

            UnityEngine.Vector3 bottomCenter = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 bottomLeft = Quaternion.Euler(0f, -angle, 0f) * UnityEngine.Vector3.forward * distance;
            UnityEngine.Vector3 bottomRight = Quaternion.Euler(0f, angle, 0f) * UnityEngine.Vector3.forward * distance;

            UnityEngine.Vector3 topCenter = bottomCenter;
            UnityEngine.Vector3 topRight = bottomRight + UnityEngine.Vector3.up * bar;
            UnityEngine.Vector3 topLeft = bottomLeft + UnityEngine.Vector3.up * bar;

            int vert = 0;

            //left side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;
            //right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segment;
            for (int i = 0; i < segment; ++i)
            {

                bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * UnityEngine.Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * UnityEngine.Vector3.forward * distance;


                topRight = bottomRight + UnityEngine.Vector3.up * bar;
                topLeft = bottomLeft + UnityEngine.Vector3.up * bar;

                //far
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                //top
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                //bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;


                currentAngle += deltaAngle;
            }


            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();


            return mesh;
        }
        private void MoveUpDown()
        {
            float updown = Mathf.PingPong(Time.time * speed, (2 * angleUPDown)) - angleUPDown;
            RadarObj.transform.localRotation = Quaternion.Euler(new UnityEngine.Vector3(updown, 0, 0));
        }
        #endregion
    }
}

