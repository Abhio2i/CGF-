using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using interfaces;
namespace Enemy.Formation
{
    public class FormationScript : MonoBehaviour
    {
        public List<GameObject> enemyPlane;
        
        public Transform leaderCraft;
        

        [HideInInspector]public float spawnCount;
        [Range(1f, 3f)]
        [SerializeField] float spread;
        private float temp;
        private float exp;


        Vector3 LHS, RHS;
        public List<Vector3> positions = new List<Vector3>();
        public Vector3 leaderOldPos;
        int count;
        private void Awake()
        {
            //enemyPlane.Clear();   
        }
        public  async void Initialize()
        {
            await CreateFormation(200f,enemyPlane.Count);
            leaderCraft.GetComponent<IFormation>().SelectLeader();
            if (enemyPlane.Count>=0)
            {
                for(int i = 0;i<enemyPlane.Count;i++)
                {
                    if (leaderCraft.gameObject == enemyPlane[i].gameObject)
                    {
                        enemyPlane.RemoveAt(i);
                        //return;
                        break;
                    }
                    enemyPlane[i].transform.position=positions[i];
                    IFormation formation = enemyPlane[i].gameObject.GetComponent<IFormation>();
                    formation.SelectSquad();
                }
            }
        }
        async Task SelectALeader() 
        {
           // await Task.Run(() =>{
                if (leaderCraft == null)
                {
                    if (enemyPlane.Count <= 0)
                        return;
                    leaderCraft = enemyPlane[0].transform;
                    leaderCraft.transform.position = leaderOldPos;
                    enemyPlane.Remove(enemyPlane[0]);
                }
            //});
            
        }
        public bool create;
        private void Update()
        {
            if (leaderCraft != null)
                leaderOldPos = leaderCraft.transform.position;
            FormationExecution();   
        }
        private async void  FormationExecution()
        {
            if (true)
            {
                if (enemyPlane.Count <= 0)
                    return;
                for (int i = enemyPlane.Count - 1; i > -1; i--)
                {
                    if (enemyPlane[i] == null)
                        enemyPlane.RemoveAt(i);
                }
                await SelectALeader();
                await CreateFormation(200f,enemyPlane.Count);
                await MoveObjectTowardsThePosition();
                //create = false;
            }
        }
        public async Task CreateFormation(float spread,int _count)//creating the V-shape formation
        {
            positions.Clear();
            count = 0;
            exp = 0;
            spawnCount = _count;
            temp = Mathf.Ceil(spawnCount / 2);
            float xPos = leaderCraft.position.x;
            float zPos = leaderCraft.position.z;
            float yPos = leaderCraft.position.y;
            //spread /= 2;
            //await Task.Run(() =>
            //{
                for (float i = xPos - 1; i >= xPos - temp; i--)
                {
                    exp = exp + 2;
                    zPos += 300f;
                    LHS = new Vector3(i - spread, yPos, zPos);
                    positions.Add(LHS);
                    count += 2;
                    if (count > spawnCount)
                        return;
                    RHS = new Vector3(i + exp + spread, yPos, zPos);
                    positions.Add(RHS);
                    spread += spread;
                }
            //});
            

        }
        async Task MoveObjectTowardsThePosition()//moving towards the formation
        {
            //await Task.Run(() =>
            //{
                for (int i = 0; i < positions.Count; i++)
                {

                    IFormation formation = enemyPlane[i].gameObject.GetComponent<IFormation>();
                    if (formation == null)
                        return;
                    formation.SelectSquad();
                    formation.FormationActive(true);
                    formation.FollowLeader(positions[i]);
                    print("hereFormation");
                    if (leaderCraft == null)
                        return;
                    leaderCraft.GetComponent<IFormation>().SelectLeader();
                }
            //});
        }

        
    }
}