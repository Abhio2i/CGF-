using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class modulescreenManager : MonoBehaviour
{

    public void screen(string s)
    {
        SceneManager.LoadScene(s);
    }
    public void exit()
    {
        Application.Quit();
    }
}
