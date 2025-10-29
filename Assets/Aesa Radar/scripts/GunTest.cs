using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GunTest : MonoBehaviour
{
    public Transform Target;

    public Transform barrelA;
    public Transform barrelB;
    public Slider slider;
    public bool refresh;
    void Start()
    {
        
    }

    public void print(int i)
    {
        refresh = true;
        StartCoroutine(upd());
    }

    // Update is called once per frame
    //void Update()
    //{
        //1018936
        //68888890


    //}

    IEnumerator upd()
    {
        yield return new WaitForSeconds(0.1f);
        if (refresh)
        {
            slider.value = 0;
            Debug.Log(1);
            yield return null;
            // Benchmark string concatenation
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            StringBuilder s = new StringBuilder(68888890);
            for (int i = 0; i < 10000000; i++)
            {
                s.Append(i.ToString());
            }
            stopwatch.Stop();
            Debug.Log("String concatenation time: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Reset();
            slider.value = 1;
            yield return null;
            // Benchmark string concatenation
            stopwatch = new Stopwatch();
            stopwatch.Start();
            s = new StringBuilder(68888890);
            for (int i = 0; i < 10000000; i++)
            {
                s.Append(i.ToString());
            }
            stopwatch.Stop();
            Debug.Log("String concatenation time: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Reset();
            slider.value = 2;
            yield return null;
            Camera.main.Render();
            // Benchmark string concatenation
            stopwatch = new Stopwatch();
            stopwatch.Start();
            s = new StringBuilder(68888890);
            for (int i = 0; i < 10000000; i++)
            {
                s.Append(i.ToString());
            }
            stopwatch.Stop();
            Debug.Log("String concatenation time: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Reset();
            slider.value = 3;
            refresh = false;
        }
    }
}
