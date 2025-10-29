using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBehaviour : MonoBehaviour,IPetrol,I_CallSign,IAvoid
{
    private bool petrol=true;

    //public UnityEngine.Vector3 RandomPos= UnityEngine.Vector3.zero;

    private bool doAvoid = false;
    private float z;
    private Quaternion rotateAngles;
    public Vector3 RandomPos=Vector3.zero;

    private GameObject MainBody;
    private UnityEngine.Vector3 direction;
    public string IFFcode;
    public GameObject ChaseBody;
    public float speed;
    void Start()
    {
        MainBody = this.gameObject;
        RandomPos.x = Random.Range(2000f, -2000f);
        RandomPos.y = Random.Range(500f, 600f);
        RandomPos.z = Random.Range(2000, -2000f);
    }
    #region interface_func
    public void StartPetrol()
    {
        petrol = !petrol;
    }
    //avoid functions
    public void AvoidObject(GameObject _obj)
    {
        ChaseBody = _obj;
        doAvoid = true;
    }

    public void StopAvoidingObject()
    {
        doAvoid = false;
    }
    //iffc code 
    public void IFFCode(string code)
    {
        IFFcode = code;
    }

    public bool IFFCode_Checker(string code)
    {
        if (IFFcode == code)
            return true;
        else
            return false;
    }
    #endregion
    private void FixedUpdate()
    {
        //avoid friendly aircraft
        if (doAvoid)
        {
            if (z < 0)
                rotateAngles = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -45f, 0));
            else
                rotateAngles = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 45f, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateAngles, 0.25f * Time.fixedDeltaTime);
            MainBody.GetComponent<Rigidbody>().velocity = MainBody.transform.forward * -speed*0.6f;
            Invoke(nameof(StopAvoidingObject), 5f);
            return;
        }

        if (petrol)
        {
            StartCoroutine(nameof(PrivodeRandomPosition));
        }
        StartRoutine(RandomPos, 0.5f);

        MainBody.GetComponent<Rigidbody>().velocity = MainBody.transform.forward * -speed;
    }
    void StartRoutine(UnityEngine.Vector3 pos, float speed)
    {
        direction = MainBody.transform.position - pos;
        Quaternion rotation = Quaternion.LookRotation(direction);
        MainBody.transform.rotation = Quaternion.Lerp(MainBody.transform.rotation, rotation, speed * Time.deltaTime);
    }
    private IEnumerator PrivodeRandomPosition()
    {
        petrol = false;
        yield return new WaitForSeconds(1f);
        UnityEngine.Vector3 dist = MainBody.transform.position - RandomPos;

        if (dist.magnitude < 100f)
        {
            RandomPos.x = Random.Range(2000f, -2000f);
            RandomPos.y = Random.Range(500f, 600f);
            RandomPos.z = Random.Range(2000, -2000f);
        }

        yield return new WaitForSeconds(0.3f);
        petrol = true;
    }
}
