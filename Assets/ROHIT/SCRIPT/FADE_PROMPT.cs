using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FADE_PROMPT : MonoBehaviour
{
    public Image Prompt;
    public Color32 Default;
    public Color32 Tranparent;
    public bool run =  false;
    public float time = 0;
    public float speed= 0;

    private void OnEnable()
    {
        Prompt.color = Default;
        run = false;
    }
    private void OnDisable()
    {
        Prompt.color = Default;
        run = false;
        Prompt.gameObject.SetActive(false);
    }

    public void FADEOUT()
    {
        if (!run)
        StartCoroutine(enumerator(false));
       
    }
     IEnumerator enumerator(bool i)
    {
        run = true;
        yield return new WaitForSeconds(time);
        run = false;
        Prompt.gameObject.SetActive(i);
    }
    private void Update()
    {
        if (run)
        {
            Prompt.color = Color.Lerp(Prompt.color, Tranparent, speed * Time.deltaTime);
        }
    }
}
