using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Vazgriz.Plane;

// Decision tree to select the maneuvers 

public class DicisonTree : MonoBehaviour
{
    /* Decision Tree  Script use target (position and face) for manuever selection according to situtaion
     */



    //public enum Manuevers { None, HardTurn, Immelmann, SplitS, LeadPursuit, LagPursuit, PurePursuit, HighSpeedGunJink, LowSpeedGunJink, Extension, Scissor, HighYoyo, LowYoyo, BreakTurn, LiftVectorTurn, GunAttack, TurningAttack, Barrelroll, Divebombing }

    public Transform enemy;
    //(locked , fire) trigger by external script (like radar and ew suite). And Facing assume 1(TRUE) for test purpose
    /* Ef = Enemy front
     * Eb = Enemy back
     * Eu = Enemy up
     * Ed = Enemy down
     * Er = Enemy right
     * El = Enemy left
     */
    public bool Ef,Eb,Eu,Ed,Er,El,locked,fire,facing=true;
    public float offset = 100,distLimit = 500;
    //private Transform ai;
    private Rigidbody rb;
    public Vector3 error;
    public Maneuvers currentManeuver;
    public bool isManeuvering=true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentManeuver = Maneuvers.none;
        //ai = transform.Find("AItransform");
        fire = true;//for test purpose
        locked = true;//for test purpose
    }
    public float direction=1;
    public bool TEST = false;
    public IEnumerator ManeuverIntervals()
    {
        print("call");
        isManeuvering = false;
        yield return new WaitForSeconds(6f);
        isManeuvering = true;
        GetComponent<AIController>().offset = new Vector3(0, 0, 50f);
        StopCoroutine(ManeuverIntervals());
    }
    // Update is called once per frame
    void Update()
    {
        if ((currentManeuver==Maneuvers.none  || isManeuvering) || TEST)
        {
            if (enemy == null)
                return;
            error = enemy.position - rb.position;
            error = (Quaternion.Inverse(rb.rotation) * error);
            if (error.magnitude > distLimit) { Ef = false; Eb = false; Eu = false; Ed = false; Er = false; El = false;currentManeuver = Maneuvers.none;
                return; }
            if (error.z > offset) { Ef = true; Eb = false; }
            else if (error.z < -offset) { Eb = true; Ef = false; }
            else { Eb = false; Ef = false; }
            if (error.y > offset) { Eu = true; Ed = false; }
            else if (error.y < -offset) { Ed = true; Eu = false; }
            else { Ed = false; Eu = false; }
            if (error.x > offset) { Er = true; El = false; }
            else if (error.x < -offset) { El = true; Er = false; }
            else { El = false; Er = false; }
            if (El)
                direction = 1;
            else if (Er)
                direction = -1;
            currentManeuver = ManeuverSelect();
        }
        
        
    }

    public Maneuvers Ask(Transform enemy)
    {
        if (enemy == null)
            return Maneuvers.none;
        error = enemy.position - rb.position;
        error = (Quaternion.Inverse(rb.rotation) * error);
        if (error.magnitude > distLimit)
        {
            Ef = false; Eb = false; Eu = false; Ed = false; Er = false; El = false; currentManeuver = Maneuvers.none;
            return Maneuvers.none;
        }
        if (error.z > offset) { Ef = true; Eb = false; }
        else if (error.z < -offset) { Eb = true; Ef = false; }
        else { Eb = false; Ef = false; }
        if (error.y > offset) { Eu = true; Ed = false; }
        else if (error.y < -offset) { Ed = true; Eu = false; }
        else { Ed = false; Eu = false; }
        if (error.x > offset) { Er = true; El = false; }
        else if (error.x < -offset) { El = true; Er = false; }
        else { El = false; Er = false; }
        if (El)
            direction = 1;
        else if (Er)
            direction = -1;
        return ManeuverSelect();
    }

    public Maneuvers ManeuverSelect(){

        if (check(new List<string>() { "u" }))
        {
            return Maneuvers.split_s;
        }
        else
        if (check(new List<string>() { "u","l" })
            || check(new List<string>() {"b","u", "l" })
            || check(new List<string>() { "b", "u", "r" }))
        {
            return Maneuvers.highYoyo;

        }
        else
        if (check(new List<string>() { "u", "r" })
            || check(new List<string>() { "r" })
            || check(new List<string>() { "d","r" })
            || check(new List<string>() { "b", "r" })
            || check(new List<string>() { "b", "d", "r" })
            || check(new List<string>() { "f", "d", "r" })
            || check(new List<string>() { "f", "d", "l" })
            || check(new List<string>() { "f", "u", "r" }))

        {
            return Maneuvers.barrelRoll;
            //return Maneuvers.leadPursuit;
        }
        else
        if (check(new List<string>() { "l" }))
        {
            return Maneuvers.hardTurn;
        }
        else
        if (check(new List<string>() { "d" })
            || check(new List<string>() { "b", "d" }))
        {
            return Maneuvers.extension;
        }
        else
        if (check(new List<string>() { "d","l" }))
        {
            return Maneuvers.lowYoyo;
        }
        else
        if (check(new List<string>() { "b" }))
        {
            return Maneuvers.breakTurn;
        }
        else
        if (check(new List<string>() { "b", "l" })
            || check(new List<string>() {"b", "d", "l" }))
        {
            return Maneuvers.scissor;
        }
        else
        if (check(new List<string>() { "b","u" })
            || check(new List<string>() { "f", "u" }))
        {
            return Maneuvers.immelmann;
            
        }
        else
        if (check(new List<string>() { "f" }))
        {
            return Maneuvers.purePursuit;
        }
        else
        if (check(new List<string>() { "f","l" })
            || check(new List<string>() { "f", "l", "u"}))
        {
            return Maneuvers.lagPursuit;
            //return Maneuvers.leadPursuit;
        }
        else
        if (check(new List<string>() { "f", "r" }))
        {
            return Maneuvers.leadPursuit;
        }
        else
         if (check(new List<string>() { "f", "d" }))
        {
            return Maneuvers.lowspeedgunjink;
        }
        else
         if (check(new List<string>() { "f", "u","r" }))
        {
            return Maneuvers.highspeedgunjink;
        }
        else
        {
            return Maneuvers.none;
        }

        //if ((!Ef && Eb && Eu && !Ed && !Er && !El && locked && fire && facing)||
        //    (!Ef && Eb && Eu && !Ed && !Er &&  El && locked && fire && facing)||
        //    (!Ef && Eb && Eu && !Ed &&  Er && !El && locked && fire && facing))
        //{
        //    return Maneuvers.highYoyo;
        //}
        //else
        //if ((!Ef && Eb && !Eu && !Ed && !Er && El && locked && fire && facing) ||
        //    (!Ef && Eb && !Eu && !Ed && Er && !El && locked && fire && facing))
        //{
        //    return Maneuvers.hardTurn;
        //}
        //else
        //if ((!Ef && Eb && !Eu && Ed && !Er && !El && locked && fire && facing)||
        //    (!Ef && Eb && !Eu && Ed && !Er && El && locked && fire && facing) ||
        //    (!Ef && Eb && !Eu && Ed && Er && !El && locked && fire && facing)||
        //    (Ef && !Eb && !Eu && Ed && !Er && !El && locked && fire && facing))
        //{
        //    return Maneuvers.lowYoyo;
        //}
        //else
        //if ((!Ef && Eb && !Eu && !Ed && !Er && !El && !locked && !fire && facing) ||
        //    (!Ef && Eb && !Eu && !Ed && !Er && El && !locked && !fire && facing) ||
        //    (!Ef && Eb && !Eu && !Ed && Er && !El && !locked && !fire && facing))
        //{
        //    return Maneuvers.breakTurn;
        //}
        //else
        //if ((!Ef && Eb && !Eu && Ed && !Er && !El && !locked && !fire && facing) ||
        //    (!Ef && Eb && !Eu && Ed && !Er && El && !locked && !fire && facing) ||
        //    (!Ef && Eb && !Eu && Ed && Er && !El && !locked && !fire && facing))
        //{
        //    return Maneuvers.split_s;
        //}
        //else
        //if ((!Ef && Eb && Eu && !Ed && !Er && !El && !locked && !fire && facing) ||
        //    (!Ef && Eb && Eu && !Ed && !Er && El && !locked && !fire && facing) ||
        //    (!Ef && Eb && Eu && !Ed && Er && !El && !locked && !fire && facing))
        //{
        //    return Maneuvers.immelmann;
        //}
        //else
        //if ((Ef && !Eb && !Eu && !Ed && !Er && !El && locked && fire && facing))
        //{
        //    return Maneuvers.lagPursuit;
        //}
        //else
        //if ((Ef && !Eb && !Eu && !Ed && !Er && El && locked && fire && facing) ||
        //    (Ef && !Eb && !Eu && !Ed && Er && !El && locked && fire && facing))
        //{
        //    //isManeuvering = false;
        //    return Maneuvers.leadPursuit;
        //}
        //else
        //{

        //    return Maneuvers.none;
        //}
    }

    public bool check(List<string> s, bool lok=true, bool fir = true,bool facin=true)
    {
        bool Ef1 ,Eb1 , Eu1 , Ed1 , Er1 , El1 , l1 , f1 , fa1 ;
        Ef1 = Ef;
        Eb1 = Eb;
        Eu1 = Eu;
        Ed1 = Ed;
        Er1 = Er;
        El1 = El;
        lok = locked;
        fir = fire;
        facin = facing;
        if (s.Contains("f"))
        {
            Ef1 = !Ef1;
        }

        if (s.Contains("b"))
        {
            Eb1 = !Eb1;
        }

        if (s.Contains("u"))
        {
            Eu1 = !Eu1;
        }

        if (s.Contains("d"))
        {
            Ed1 = !Ed1;
        }

        if (s.Contains("r"))
        {
            Er1 = !Er1;
        }

        if (s.Contains("l"))
        {
            El1 = !El1;
        }


        return (!Ef1 && !Eb1 && !Eu1 && !Ed1 && !Er1 && !El1);// && !lok && !fir && !facin);
    }

}
