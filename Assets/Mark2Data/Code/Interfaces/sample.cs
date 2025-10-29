using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sample : MonoBehaviour
{
    [SerializeField] GameObject follow;
    [SerializeField] GameObject _follow;
    [SerializeField] GameObject UI;
    private void Start()
    {
        GameObject obj=Instantiate(UI);
        UI=obj;
        obj.transform.SetParent(_follow.transform, false);
    }
    private void Update()
    {
        UI.transform.position=follow.transform.position;
    }
}
