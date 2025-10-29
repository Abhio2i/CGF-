using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject otherObject;
    [SerializeField] GameObject[] ActivateObjects;
    [SerializeField] string []message = new string[5];
    [HideInInspector] public bool changeScene;
    public TMP_Text updateText;
    //public static LoadSystemAndSpawnPlanes instance;
    private int factor;
    private void Start()
    {
        //return;
        PlayerPrefs.SetInt("Progress", 0);
        canvas.SetActive(true);
        factor = 0;
        Invoke(nameof(InvokeSpawnGameObject), 3f);
        StartCoroutine(MyRoutine());
        Time.timeScale = 1f;        //CODE ADDED
    }
    void InvokeSpawnGameObject()
    {
        if (otherObject == null)
            return;
        otherObject.SetActive(true);
        //Time.timeScale = 0f;
    }
    IEnumerator MyRoutine()
    {
        while (false)        //factor<=3 CODE ADDED
        {
            factor = PlayerPrefs.GetInt("Progress");
            slider.value = factor * 0.4f;
            updateText.text = "Loading...";
            if(factor == 0) 
            {
                //load Enemy Planes.
                updateText.text = message[0];// "Loading Enemy Planes";

            }
            else if(factor == 1) 
            {
                //load Ally Planes.
                updateText.text = message[1];// "Loading Ally Planes";
            }
            else if(factor == 2) 
            {
                //load Neutral Planes.
                updateText.text = message[2];// "Loading Neutral Planes";
            }
            else if(factor == 3) 
            {
                //load Neutral Planes.
                updateText.text = message[3];// "Loading Ai Planes";
            }
            else 
            {
                //Loading Complete
                updateText.text = message[4];// "Loading Complete";
                changeScene = true;
            }
            yield return null;
        }
        Time.timeScale = 1f;

        yield return new WaitForSeconds(3f);
        try
        {
           Destroy(canvas);
            //gameObject.GetComponent<WinLoose>().enabled = true;
        }
        catch { };
        if(ActivateObjects.Length > 0)
            foreach(var obj in ActivateObjects)
            {
                yield return new WaitForEndOfFrame();
                obj.SetActive(true);
            }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Deactivate();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlane();
        }
    }
    private void LoadPlane()
    {
        foreach (var obj in ActivateObjects)
        {
            obj.SetActive(true);
        }
    }
    private void Deactivate()
    {
        foreach (var obj in ActivateObjects)
        {
            obj.SetActive(false);
        }
    }
}
