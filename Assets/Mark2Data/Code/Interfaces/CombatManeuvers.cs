using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManeuvers : MonoBehaviour, ICombatManeuvers
{
    public Transform player;
    [Header(" ")]
    [SerializeField] bool hardTurn;
    [SerializeField] bool leadPursuit;
    [SerializeField] bool lagPursuit;
    [SerializeField] bool purePursuit;
    [SerializeField] bool splitS;
    [SerializeField] bool highYoYo;
    [SerializeField] bool lowYoYo;
    [SerializeField] bool immelmann;
    [SerializeField] bool barellRoll;
    [SerializeField] bool diveBomb;

    private Vector3 posToHardTurn;
    private Vector3 posToFollow;
    private Vector3 posTohighYoyo;
    private Vector3 eulerAngles;
    private Vector3 oldPosition;
    private float magnitude;
    private bool isCombatTrigger;
    private bool passOnce = true;
    private GameObject childBody;
    private IEnumerator _timer;
    public void isHardTurn()
    {
        hardTurn = !hardTurn;
    }
    public void isYoYo(float type)
    {
        if (type == 1)
            highYoYo = !highYoYo;
        else
            lowYoYo = !lowYoYo;
    }
    public void isBarrelroll()
    {
        barellRoll=!barellRoll;
    }
    public void isLead()
    {
        leadPursuit = !leadPursuit;
    }
    public void isLag()
    {
        lagPursuit = !lagPursuit;
    }
    public void isPure()
    {
        purePursuit = !purePursuit;
    }
    public void isSplit_S()
    {
        splitS = !splitS;
    }
    public void isImmelmann()
    {
        immelmann = !immelmann;
    }
    public void isDiveBomb()
    {
        diveBomb = !diveBomb;
    }
    void Start()
    {
        childBody = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {

        ////if(scissorRoll)
        //{
        //    ScissorRoll();
        //    return;
        //}
        if (player == null)
            return;
        if(barellRoll)
        {
            Barrelroll();
            return;
        }
        if(highYoYo)
        {
            YoYo(1f);
            return;
        }
        if(lowYoYo)
        {
            YoYo(-1f);
            return;
        }
        if (hardTurn)
        {
            HardTurn();
            return;
        }
        if (leadPursuit)
        {
            Lead();
            return;
        }
        if(lagPursuit)
        {
            Lag();
            return;
        }
        if(purePursuit)
        {
            Pure();
            return;
        }
        if (splitS)
        {
            Split_S();
            return;
        }
        if(immelmann)
        {
            Immelmann();
            return;
        }
        if(diveBomb)
        {
            DiveBomb();
            return;
        }
     }
    #region hard turn script
    //hard turn
    float duration;
    Vector3 childRotation;
    public void HardTurn()
    {
        if (passOnce)
        {
            posToHardTurn = transform.position - new Vector3(0, 0, -100f);
            oldPosition = transform.position;
            duration = 5f;
            passOnce = !passOnce;
        }

        duration -= Time.deltaTime;

        if (duration > 0)
        {
            childRotation = new Vector3(0, 0, -80f);
        }
        else
        {
            posToHardTurn = player.position;
            childRotation = Vector3.zero;
            Invoke(nameof(DisableHardTurn), 10f);
        }
        Rotation(posToHardTurn, childRotation);      
    }

    public void DisableHardTurn()
    {
        passOnce = !passOnce;
        hardTurn = false;
    }
    #endregion
    #region lead,lag,pure pursuit
    //lead lag pure pursuit
    public void Lead()
    {
            posToFollow = player.position + new Vector3(0, 0, 500f);
        Rotation(posToFollow, Vector3.zero);
    }
    public void Lag()
    {
        posToFollow = player.position + new Vector3(0, 0, -500f);
        Rotation(posToFollow, Vector3.zero);
    }
    public void Pure()
    {
        posToFollow = player.position;

        Rotation(posToFollow, Vector3.zero);
    }
    #endregion
    #region Split_S
    bool firstStage = true;
    bool secondStage = true;
    bool thirdStage=true;
    bool zeroStage=true;
    Vector3 splitAngle = Vector3.zero;
    public float splitDuration = 40f;
    public void Split_S()
    {
        if(splitDuration<=0)
        {
            ResetSplit_S();
            splitS = false;
            splitDuration = 40f;
            return;
        }    
        splitDuration -= Time.fixedDeltaTime;
        if(splitDuration>=31f && splitDuration<40f && zeroStage)
        {
            splitAngle = new Vector3(0, transform.eulerAngles.y, 0);
            zeroStage = !zeroStage;
        }
        if (splitDuration > 21f && splitDuration <= 30f && firstStage)
        {
            splitAngle = new Vector3(-180f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            firstStage = !firstStage;
        }
        else if (splitDuration > 11f && splitDuration <= 20f && secondStage)
        {
            splitAngle = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z+180f);
            secondStage = !secondStage;
        }
        else if(splitDuration <=10f && thirdStage)
        {
            splitAngle = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            thirdStage=!thirdStage;
            return;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(splitAngle),0.45f*Time.deltaTime);
    }

    public void ResetSplit_S()
    {
        firstStage = !firstStage;
        secondStage= !secondStage;
        thirdStage= !thirdStage;
        zeroStage=!zeroStage;
    }
    #endregion
    #region immelmann
    public void Immelmann()
    {
        if (splitDuration <= 0)
        {
            ResetSplit_S();
            immelmann = false;
            splitDuration = 40f;
            return;
        }
        splitDuration -= Time.fixedDeltaTime;
        if (splitDuration >= 31f && splitDuration < 40f && zeroStage)
        {
            splitAngle = new Vector3(0, transform.eulerAngles.y, 0);
            zeroStage = !zeroStage;
        }
        if (splitDuration > 21f && splitDuration <= 30f && firstStage)
        {
            splitAngle = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 180f);
            firstStage = !firstStage;
        }
        else if (splitDuration > 11f && splitDuration <= 20f && secondStage)
        {
            splitAngle = new Vector3(180f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            secondStage = !secondStage;
        }
        else if (splitDuration <= 10f && thirdStage)
        {
            splitAngle = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            thirdStage = !thirdStage;
            return;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(splitAngle), 0.45f * Time.deltaTime);
    }
    #endregion
    #region yoyo
    bool Yoyo_stage1=true;
    bool Yoyo_stage2=true;
    int direction;
    public void YoYo(float type)
    {
        if (player.position.x - transform.position.x > 0)
            direction = 1;
        else
            direction = -1;
        if (Yoyo_stage1)
        {
            posTohighYoyo = transform.position + new Vector3(direction*40f, type*400f, -200f);
            //childRotation = transform.rotation.eulerAngles + new Vector3(0, 0, -direction*170f);
            Yoyo_stage1 = !Yoyo_stage1;
        }
        if(Vector3.Distance(transform.position,posTohighYoyo)<=10f && Yoyo_stage2)
        {
            posTohighYoyo = player.transform.position + new Vector3(0, 0, -100f);
            childRotation = Vector3.zero;
            Yoyo_stage2 = !Yoyo_stage2;
            Invoke(nameof(DisableYoYo), 10f);
        }
        Rotation(posTohighYoyo, childRotation);
    }

    public void DisableYoYo()
    {
        Yoyo_stage1 = !Yoyo_stage1;
        Yoyo_stage2=!Yoyo_stage2;
        highYoYo = lowYoYo = false;
    }
    #endregion
    #region scissors flat roll
    bool scissors_zeroStage=true;
    Vector3 _lagPursuit_Player;
    bool scissors_firstStage=true;
    Vector3 myAngle;
    bool pass = true;
    public void ScissorRoll()
    {
        if(pass)
        {
            myAngle = transform.rotation.eulerAngles + new Vector3(0, 230f,0);
            Invoke(nameof(Deactivate), 5f);
            pass = false;
        }
        if (scissors_zeroStage)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(myAngle), 0.26f * Time.deltaTime);
            childBody.transform.localRotation = Quaternion.Slerp(childBody.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -80f)), 0.35f * Time.deltaTime);
            return;
        }
        if(scissors_firstStage)
        {
            _lagPursuit_Player = player.transform.position + new Vector3(0, 0, -30f);
            //scissors_firstStage = false;
        }
        Rotation(_lagPursuit_Player,Vector3.zero);
    }

    public void Deactivate()
    {
        scissors_zeroStage = false;
        pass=true;
    }
    #endregion
    #region barrel roll
    [SerializeField]Vector3 _angleBarrelRoll;
    Vector3 posAngle;
    // float barrelRoll_duration = 20f;
    IEnumerator BarrelRoll_Rout;
    public float sum;
    public void Barrelroll()
    {
        Debug.Log(transform.rotation.eulerAngles.z);
        _angleBarrelRoll = transform.rotation.eulerAngles;
        _angleBarrelRoll.z = _angleBarrelRoll.z + 30f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_angleBarrelRoll), 0.75f * Time.deltaTime);
        //Invoke(nameof(DisableBarrelRolls), 17f);
    }
    public void DisableBarrelRolls()
    {
        barellRoll = false;
    }
    #endregion
    #region divebomb
    bool diveBomb_stage1=true;
    bool diveBomb_stage2=true;
    bool diveBomb_stage3=false;
    float oldPos;
    [SerializeField]Vector3 diveBomb_angle;
    public void DiveBomb()
    {
        if (diveBomb_stage1)
        {
            diveBomb_angle=new Vector3(transform.rotation.eulerAngles.x+75f,0f,0f);
            oldPos = transform.position.y;
            if(diveBomb_angle.x>180f)
            {
                diveBomb_angle.x -= 360f;
            }
            diveBomb_stage1 = !diveBomb_stage1;
        }
        if((diveBomb_stage2) && (transform.position.y-oldPos)>=300f)
        {
            diveBomb_angle = new Vector3(transform.rotation.eulerAngles.x - 150f, 0f, 0f);
            if (diveBomb_angle.x > 180f)
            {
                diveBomb_angle.x -= 360f;
            }
            diveBomb_stage2 = !diveBomb_stage2;
            Invoke(nameof(FollowPlayer),7f);
        }
        if(diveBomb_stage3)
        {
            Rotation(player.transform.position, Vector3.zero);
            //Invoke(nameof(DisableDiveBomb), 5f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(diveBomb_angle.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0.78f * Time.deltaTime);
    }
    public void FollowPlayer()
    {
        diveBomb_stage3 = !diveBomb_stage3;
    }
    public void DisableDiveBomb()
    {
        diveBomb = false;
        diveBomb_stage1 = !diveBomb_stage1;
        diveBomb_stage2=!diveBomb_stage2;
        diveBomb_stage3 = !diveBomb_stage3;
    }
    #endregion
    float timer;
    public void Rotation(Vector3 pos,Vector3 childRotation)
    {
        Vector3 direction = transform.position - pos;
        Quaternion _rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rot, 0.85f * Time.deltaTime);

        //transform.position += transform.forward * -20f * Time.deltaTime;
        Quaternion _child = Quaternion.Euler(childRotation);
        
        childBody.transform.localRotation = Quaternion.Slerp(childBody.transform.localRotation, _child, 0.5f * Time.deltaTime);
    }
}
