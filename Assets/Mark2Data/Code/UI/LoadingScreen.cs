#region Script Info
//out: Display Loading Screen for 3 seconds when the Simulator i going to start.
#endregion
using System.Collections;
using UnityEngine;
using TMPro;
public class LoadingScreen : MonoBehaviour
{
    #region Global Perimeters
    public GameObject loadingScreen;
    public TMP_Text loaing;
    #endregion

    #region local Perimeters
    private bool isTextDisplay = false;
    #endregion

    #region Local Functions
    void Start()
    {
        isTextDisplay = true;
        PlayerPrefs.DeleteAll();
        Invoke(nameof(StartLoading), 3f);
    }

    private void StartLoading()
    {
        isTextDisplay = false;
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        if (isTextDisplay) 
        {
            StartCoroutine(BlinkLoading());
        }
    }

    private IEnumerator BlinkLoading() 
    {
        loaing.text = "Loading.";
        yield return new WaitForSeconds(0.01f);
        loaing.text = "Loading..";
        yield return new WaitForSeconds(0.01f);
        loaing.text = "Loading...";
        yield return new WaitForSeconds(0.01f);
        loaing.text = "Loading....";
    }
    #endregion
}
