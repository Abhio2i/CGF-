using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTargets : MonoBehaviour
{
    public Transform target;
    public string EnemyTag;
    public bool iAmPlayer;
    public Transform[] FlareTargets=new Transform[4];
    private int chooseTarget;
    public void Start()
    {
        Invoke(nameof(GiveTarget), 1f);
    }
    public void GiveTarget()
    {
        chooseTarget = Random.Range(1, 10);
        if (FlareTargets.Length>0)
        {
            if (chooseTarget % 2 == 0)
                gameObject.GetComponent<MissileSystem>().target = target;
            else
            {
                int k = Random.Range(0, FlareTargets.Length);
                if (FlareTargets[k]==null)
                {
                    gameObject.GetComponent<MissileSystem>().target = target;
                    return;
                }
                Transform flaretarget=FlareTargets[k];
                gameObject.GetComponent<MissileSystem>().target = flaretarget;
            }
        }
        else
        {
            gameObject.GetComponent<MissileSystem>().target = target;
        }
    }
}
