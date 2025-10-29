using Assets.Scripts.Feed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Camera cam;
    public GameObject DisplayBoard,ScrollView,player, ally, enemy, playerMissile, allyMissile, enemyMissile;
    public List<GameObject> aircrafts,ui,missiles;
    private List<LineRenderer> canvasTrails=new List<LineRenderer>();
    private List<SetEntityPositionAndRotation> missiledata = new List<SetEntityPositionAndRotation>();
    private bool AllSet;
    Vector3 r,frd;
    float w,h;
    private void Start()
    {
        r = cam.ViewportToScreenPoint(new Vector2(1, 1));
        w = gameObject.GetComponent<CanvasScaler>().referenceResolution.x;
        h = (w / (r.x / r.y)); 
    }
    void OnEnable()
    {
        cam.gameObject.SetActive(true);
        if (!AllSet)
        {
            foreach (var aircraft in aircrafts)
            {
                if (aircraft.name.Contains("Player"))
                    ui.Add(Instantiate(player, transform));
                if (aircraft.name.Contains("Enemy"))
                    ui.Add(Instantiate(enemy, transform));
                if (aircraft.name.Contains("Ally"))
                    ui.Add(Instantiate(ally, transform));
                canvasTrails.Add(aircraft.GetComponent<LineRenderer>());
            }
            foreach (var missile in missiles)
            {
                if (missile.name.Contains("My"))
                    ui.Add(Instantiate(playerMissile, transform));
                if (missile.name.Contains("Bad"))
                    ui.Add(Instantiate(enemyMissile, transform));
                if (missile.name.Contains("Good"))
                    ui.Add(Instantiate(allyMissile, transform));
                missiledata.Add(missile.GetComponent<SetEntityPositionAndRotation>());
                canvasTrails.Add(missile.GetComponent<LineRenderer>());
            }
        }
        DisplayBoard.SetActive(false);
        ScrollView.SetActive(false);
        AllSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < aircrafts.Count; i++)
        {
            frd = cam.WorldToViewportPoint(aircrafts[i].transform.position);
            frd.x = frd.x * w - (w / 2);
            frd.y = frd.y * h - (h / 2);
            if (frd.z < 0) {frd.x = 10000; }
            frd.z = 1f;
            //frd = new Vector3(frd.x - Screen.currentResolution.width / 2, frd.y - Screen.currentResolution.height/2, 0);
            ui[i].transform.localPosition = frd;
            //ui[i].transform.GetChild(0).localRotation = Quaternion.LookRotation(cam.transform.right,cam.transform.position-transform.position);
            ui[i].transform.rotation =aircrafts[i].transform.rotation;
        }
        for (int i = 0; i < missiles.Count; i++)
        {
            if (!missiledata[i].iAmActive) { ui[aircrafts.Count + i].SetActive(false); }
            else
            {
                ui[aircrafts.Count + i].SetActive(true);
                frd = cam.WorldToViewportPoint(missiles[i].transform.position);
                frd.x = frd.x * w - (w / 2);
                frd.y = frd.y * h - (h / 2);
                if (frd.z < 0) { frd.x = 10000; }
                frd.z = 1f;
                //frd = new Vector3(frd.x - Screen.currentResolution.width / 2, frd.y - Screen.currentResolution.height/2, 0);
                ui[aircrafts.Count + i].transform.localPosition = frd;
                //ui[i].transform.GetChild(0).localRotation = Quaternion.LookRotation(cam.transform.right,cam.transform.position-transform.position);
                ui[aircrafts.Count + i].transform.rotation = missiles[i].transform.rotation;
            }
        }
        int s = 0;
        foreach (var trail in canvasTrails)
        {
            if (trail.gameObject.name.Contains("Missile"))
            {
                if (!missiledata[s].iAmActive) { trail.widthMultiplier = 1; }
                else { trail.widthMultiplier = (int)cam.transform.position.y / 250; }
                s++;
            }
            else { trail.widthMultiplier = (int)cam.transform.position.y / 350; }
        }
    }
    private void OnDisable()
    {
        foreach (var trail in canvasTrails)
        {
            if(trail!=null)
            trail.widthMultiplier = 1;
        }
        if(DisplayBoard!=null)
        DisplayBoard.SetActive(true);
        if(ScrollView!=null)
        ScrollView.SetActive(true);
        if(cam!=null)
        cam.gameObject.SetActive(false);
    }
}
