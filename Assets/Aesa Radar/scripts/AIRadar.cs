using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AIRadar : MonoBehaviour
{
    public float power = 763;
    public float aperture = 1;
    public float Range = 0.5f;
    public float azimuth = 30f;
    public float bar = 30f;
    public int f=0, G=0;
    private float az = 0f;
    private float br = 0f;
    private float rg = 0f;
    private bool jam;
    Mesh mesh;
    MeshCollider meshCollider;
    public Vector3[] ver;
    public List<List<int>> item = new List<List<int>>();
    public List<string> TargetTag = new List<string>();
    public List<GameObject> DetectedObjects = new List<GameObject>();
    public Dictionary<int,GameObject> objectLookup = new Dictionary<int,GameObject>();
    public NativeHashSet<int> activeIDs;
    public List<GameObject> SightObjects = new List<GameObject>();
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    private int clock=1;
    void Start()
    {
        activeIDs = new NativeHashSet<int>(100, Allocator.Persistent);
        loadSystemAndSpawnPlanes = FindObjectOfType<LoadSystemAndSpawnPlanes>();
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        ver = mesh.vertices;
        similarver();
        StartCoroutine(AntiJam());
        Invoke("updateRadar", 2f);
    }

    private void Update()
    {
        List<GameObject> list = new List<GameObject>();
        foreach(GameObject gameObject in DetectedObjects)
        {
            if(gameObject != null && gameObject.transform.root.gameObject.activeSelf)
            {
                list.Add(gameObject);
            }
        }
        DetectedObjects = new List<GameObject>();
        DetectedObjects.AddRange(list);



    }

    private void FixedUpdate()
    {
        if (rg != Range || azimuth != az || bar != br)
        {
            rg = Range;
            az = azimuth;
            br = bar;
            updateRadar();
            //Debug.Log("update");
        }

        
    }
    public void similarver()
    {
        List<Vector3> s = new List<Vector3>();
        for (int i = 0; i < ver.Length; i++)
        {
            if (!s.Contains(ver[i]))
            {
                s.Add(ver[i]);
            }
        }

        for (int i = 0; i < s.Count; i++)
        {
            bool f = false;
            for (int v = 0; v < ver.Length; v++)
            {
                if (ver[v] == ver[i])
                {
                    if (!f)
                    {
                        f = true;
                        item.Add(new List<int>());
                    }
                    item[item.Count - 1].Add(v);
                }
            }
        }
    }

    private void upvertex(int index, Vector3 v)
    {
        var d = item[index];
        for (int i = 0; i < d.Count; i++)
        {
            ver[d[i]] = v;
        }
    }

    private Vector3 getvertex(int index)
    {
        var d = item[index][0];
        return ver[d];
    }




    public void updateRadar()
    {
        meshCollider.enabled = false;
        // 0 front right down index 
        // 1 front left down index
        // 2 front right up index
        // 3 front left up index
        var x = Range * Mathf.Tan(Mathf.Deg2Rad * azimuth);
        var y = Range * Mathf.Tan(Mathf.Deg2Rad * bar);
        upvertex(0, new Vector3(0.5f + x, -0.5f - y, Range - 0.5f));
        upvertex(1, new Vector3(-0.5f - x, -0.5f - y, Range - 0.5f));
        upvertex(2, new Vector3(0.5f + x, 0.5f + y, Range - 0.5f));
        upvertex(3, new Vector3(-0.5f - x, 0.5f + y, Range - 0.5f));
        mesh.vertices = ver;
        //mesh.Optimize();
        meshCollider.convex = true;
        meshCollider.enabled = true;
        DetectedObjects.Clear();
        //Invoke("UpdateObject",0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (/*!activeIDs.Contains(other.gameObject.GetInstanceID())*/!DetectedObjects.Contains(other.gameObject) && TargetTag.Contains(other.tag))
        {
            if (true)//!(SightObjects.Contains(other.gameObject) && jam))
            {
                //activeIDs.Add(other.gameObject.GetInstanceID());
                //objectLookup.Add(other.gameObject.GetInstanceID(), other.gameObject);
                DetectedObjects.Add(other.gameObject);
                string targt = loadSystemAndSpawnPlanes.findCraftType(other.gameObject);
                string By = loadSystemAndSpawnPlanes.findCraftType(gameObject);
                FeedBackRecorderAndPlayer.AddEvent(By + " Detect " + targt);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (/*activeIDs.Contains(other.gameObject.GetInstanceID()))*/DetectedObjects.Contains(other.gameObject))
        {
            //activeIDs.Remove(other.gameObject.GetInstanceID());
            DetectedObjects.Remove(other.gameObject);
        }
    }
    public float JamSignal(Jammer.JamMode mode,float frequency,float gain, GameObject jammer,float jamSignal)
    {
        float Pt = power;
        float Gr = G;
        float D = Vector3.Distance(jammer.transform.root.position, transform.root.position) * 2;
        float σ = aperture;
        float F = f * 1000000000f;

        var PT = 10 * Mathf.Log10(Pt / 0.001f);//watt to dBm
        float S = PT + (2 * Gr) - 103 - (20 * Mathf.Log(F)) - (40 * Mathf.Log(D)) + (10 * Mathf.Log(σ));
        S = PT + Gr + (20 * Mathf.Log10(D));
        float JS = jamSignal - S;
        if (mode == Jammer.JamMode.Barring)
        {
            if ((int)gain > G)
            {
                DetectedObjects.Clear();
            }

            return JS;
        }

        if ((frequency == f || mode == Jammer.JamMode.DRFMjamming) && JS > 1)//JamValid or Not
        {

            var jamAngle = Vector3.SignedAngle(jammer.transform.position - transform.position, transform.forward, transform.up);
            foreach (GameObject aircraft in DetectedObjects)
            {
                var anglediff = Vector3.SignedAngle(aircraft.transform.position - transform.position, transform.forward, transform.up) - jamAngle;
                if ((anglediff > 0 ? anglediff : -anglediff) < 3)
                {
                    if (!SightObjects.Contains(aircraft))
                        StartCoroutine(JamIT(aircraft));
                }
            }
            foreach (GameObject aircraft in SightObjects)
            {
                jam = true;
                //DetectedObjects.Remove(aircraft);
            }
        }
        else { SightObjects.Clear(); }
        return JS;
    }
    IEnumerator JamIT(GameObject aircraft)
    {
        yield return new WaitForSeconds(0.1f);
        SightObjects.Add(aircraft);
    }
    IEnumerator AntiJam()
    {
        yield return new WaitForSeconds(15);
        if (jam) { f = (f + 1) % 8; f = f > 4 ? 0 : f; jam = false; SightObjects.Clear(); }
        StartCoroutine(AntiJam());
    }
}
