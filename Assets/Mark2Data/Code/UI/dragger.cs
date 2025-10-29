using UnityEngine;

public class dragger : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    //public Camera cam;
    public int index;
    public bool isAlly, isSAMS, isEnemy;
    public MasterSpawn master;
    bool move = false;
    public LayerMask layermask;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                    move = true;
                }
            }
        }

        if (Input.GetMouseButton(0) && move)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition.y = transform.position.y;
            transform.position = curPosition;
        }
        if (Input.GetMouseButtonUp(0) && move)
        {
            move = false;
            if (PlayerPrefs.GetInt("Scenerio") == 2)
                new Vector3(transform.position.x, 60, transform.position.z);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit,100000, layermask);
            if (isAlly) { master.ally_spawnPlanes[index].spawnPosition =       new Vector3(transform.position.x,hit.point.y,transform.position.z); }
            if (isEnemy) { master.adversary_spawnPlanes[index].spawnPosition = new Vector3(transform.position.x,hit.point.y,transform.position.z); }
            if (isSAMS) { master.sams_spawnPlanes[index].spawnPosition =       new Vector3(transform.position.x,hit.point.y, transform.position.z); }
            if (PlayerPrefs.GetInt("Scenerio") == 2 && isSAMS)
                master.sams_spawnPlanes[index].spawnPosition = new Vector3(transform.position.x, 60, transform.position.z);
        }
    }
     
}