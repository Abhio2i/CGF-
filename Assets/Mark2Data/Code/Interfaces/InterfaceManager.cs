using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Follow
{
    void CoroutineSyntax();
}

public interface IAvoid
{
    void AvoidObject(GameObject obj);

    void StopAvoidingObject();
}

public interface IDamage
{
    void Count();
}

public interface IHUD
{
    void Create();
    void Destroy();
}

public interface IFollow 
{
    public void StartFollow();
}

public interface IPetrol 
{
    public void StartPetrol();
}

public interface IChase 
{
    public void StartChase(GameObject Body);

    public void StopChase();
}

public interface IRun 
{
    public void StartRun();
    public void StopRun();
}

public interface IWeapon 
{
    public void StartFire(GameObject Body);
}
public interface ICombatManeuvers
{
    public void isHardTurn();
    public void isYoYo(float type);
    public void isBarrelroll();
    public void isLead();
    public void isLag();
    public void isPure();
    public void isSplit_S();
    public void isImmelmann();
    public void isDiveBomb();
}
public interface I_CallSign
{
    public void IFFCode(string iffcode);
    public bool IFFCode_Checker(string iffcode);
}