//creating circle formation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using interfaces;
namespace Enemy.Formation
{
    public class C_Formation : MonoBehaviour
    {
        [SerializeField]public int amount;
        [SerializeField] float G;

        public Vector3 aroundPoint;
        public List<GameObject> enemyPlanes = new List<GameObject>();
        public int radius;
        public float index = 0;
        public float incrementAngle;
        public float rotationSpeed;
        public int previousCount;
        public int currentCount;
        public GameObject refPoint;

        List<GameObject> temp=new List<GameObject>();
        public List<Vector3> pos = new List<Vector3>();
        Dictionary<GameObject, GameObject> planes = new Dictionary<GameObject, GameObject>();

        public void Initialize()
        {
            aroundPoint=new Vector3(aroundPoint.x,500f,aroundPoint.y);
            refPoint = new GameObject();
            incrementAngle = 360f / amount;
            
            CreateEnemiesAroundPoint(amount,aroundPoint,radius,false);
            for (int i = 0; i < temp.Count; i++)
            {
                enemyPlanes[i].transform.position = temp[i].transform.position;

            }
            previousCount =planes.Count;
            currentCount = planes.Count;
        }
        private void FixedUpdate()
        {
            if(previousCount!=enemyPlanes.Count)
            {
                ResetPoints();
                CreateEnemiesAroundPoint(enemyPlanes.Count, aroundPoint, radius,false);
                previousCount = enemyPlanes.Count;
            }
            //foreach()
            if (enemyPlanes.Count > 0) //rotate the planes
            {
                for(int i=0;i<enemyPlanes.Count;i++)
                {
                    if (enemyPlanes[i] == null)
                    {
                        enemyPlanes.RemoveAt(i);
                    }
                    else
                    {
                        temp[i].transform.RotateAround(aroundPoint, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
                        //enemyPlanes[i].transform.position = Vector3.Lerp(enemyPlanes[i].transform.position,temp[i].transform.position,0.35f*Time.deltaTime);

                        IFormation formation = enemyPlanes[i].GetComponent<IFormation>();
                        if (formation == null)
                            return;
                        formation.SelectSquad();
                        formation.FormationActive(true);
                        formation.FollowLeader(temp[i].transform.position);
                    }
                }
            }
        }
        void ResetPoints()
        {
            foreach(GameObject gameObject in temp)
            {
                Destroy(gameObject);
            }
            temp.Clear();
        } //reset 
        public void CreateEnemiesAroundPoint(int num, Vector3 point, float radius,bool ignore)
        {
            temp.Clear();
            for (int i = 0; i < num; i++)
            { 
                /* Distance around the circle */
                var radians = i * 2 * Mathf.PI / num;

                /* Get the vector direction */
                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians);

                var spawnDir = new Vector3(horizontal, 0, vertical);

                /* Get the spawn position */
                var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point
                pos.Add(spawnPos);

                if (!ignore)
                {
                    var _object = Instantiate(refPoint, spawnPos, Quaternion.identity);

                    temp.Add(_object);
                }
            }
        } //create formation
    }
}