using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerController : MonoBehaviour
{
    [Header("speed in meter per seconds")]
    [SerializeField]float speed;

    [SerializeField] Transform turret;

    [SerializeField] Transform missile;

    [SerializeField]CarrierShipController carrier;

    public bool move;

    public Transform target;

    private Transform firingMissile;
    void Fire()
    {
        if (firingMissile == null)
        {
            firingMissile = Instantiate(missile);
            firingMissile.transform.position = turret.position + new Vector3(0, 4f, 5f);
            firingMissile.transform.rotation = turret.rotation;
            firingMissile.gameObject.SetActive(true);
        }
    }
    void Rotate()
    {
        SelectTarget();
        if(target==null)return;
        Vector3 targetDirection = target.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(turret.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        turret.rotation = Quaternion.LookRotation(newDirection);
    }
    void SelectTarget()
    {
        //target =
        //    carrier.targets[Random.Range(0, carrier.targets.Count - 1)];
    }
    public void UpdateDestroyerFunctions()
    {
        //Rotate();
        //Fire();
    }

}
