using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SwitchScene : MonoBehaviour
{
    public void ChangeScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        var add = PlayerPrefs.GetInt("EnemySpawnCount") > 0? 0 : 1 ;
        add = index == 0 ? add : 0;
        SceneManager.LoadScene(index + 1+add);
        //Debug.Log(add);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
