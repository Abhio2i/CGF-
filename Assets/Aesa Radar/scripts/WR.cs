
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WR : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public ParticleSystem m_System;
    public ParticleSystem.Particle[] m_Particles;

    public int w = 400;
    public int h = 400;
    public int w2 = 400;
    public float size = 1f;
    public RawImage image;
    public Texture2D t;
    private int[] ok;
    Bounds b;

    private BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
        t = new Texture2D(w, h, TextureFormat.ARGB32, false);
        collider = GetComponent<BoxCollider>();
        b = collider.bounds;
        
    }

    // Update is called once per frame
    void Update()
    {
        t.SetPixels(0, 0, w, h, new Color[w*h]);
        var d = t.GetPixels32();
        ok = new int[w2*w2];
        //dS = new int[w * h];
        //Debug.Log(dS[0]);
        foreach (var part in particleSystems)
        {
            var pos = part.transform.position;
            int numParticlesAlive = part.GetParticles(m_Particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                if (b.Contains(m_Particles[i].position))
                {
                    int block = 1;

                    // create an array for our colors
                    //Color[] colorArray = new Color[block*block];

                    // fill this with our color
                    //for (int d = 0; d < block; ++d)
                    //{ colorArray[d] = Color.red; }
                    var p = transform.InverseTransformPoint(m_Particles[i].position+pos);
                    var x = Mathf.FloorToInt((p.x + (size / 2f)) * (w2 - block));
                    var y = Mathf.FloorToInt((p.z + (size / 2f)) * (w2 - block));
                    var index = x + (y * w2);

                    //t.SetPixels(Mathf.FloorToInt((x.x + (size/2f)) * (w-block)), Mathf.FloorToInt((x.z + (size / 2f)) * (h - block)), block, block, colorArray);
                    //var d = t.GetPixels(Mathf.FloorToInt((x.x + (size / 2f)) * (w - block)), Mathf.FloorToInt((x.z + (size / 2f)) * (h - block)), block, block);
                    //for(int j = 0; j < d.Length; j++)
                    //{
                    //    d[j][0] = 255; 
                    //    d[j][1] = 255;
                    //    d[j][2] = 255;
                    //    d[j][3] = 255;
                    //}
                    //var ii = (int)d[index][0];

                        if (index<w2*w2 && index>=0) {
                            var ii = ok[index];
                            if (ii < 248)
                            {
                                ok[index] += 5;
                            }
                        }

                    
                        //var ds = (byte)(ii + 50);
                        //d[index][0] = ds;
                        //d[index][1] = ds;
                        //d[index][2] = ds;
                        //d[index][3] = 255;
                        //if (x < w - 5&& y <h-5)
                        //{
                        //    Color[] colorArray = new Color[25];

                        //    // fill this with our color
                        //    for (int pk = 0; pk < 25; pk++)
                        //    {
                        //        var f = (int)ds;

                        //        if (f > 9 && f <= 100)
                        //        {
                        //            colorArray[pk] = Color.grey;
                        //        }
                        //        else if (f > 100 && f <= 150)
                        //        {
                        //            colorArray[pk] = Color.green;
                        //        }
                        //        else if (f > 150 && f <= 200)
                        //        {
                        //            colorArray[pk] = Color.yellow;
                        //        }
                        //        else if (f > 200)
                        //        {
                        //            colorArray[pk] = Color.red;
                        //        }
                        //    }
                        //    x = (int)(x / 5f);
                        //    y = (int)(y / 5f);
                        //    t.SetPixels(x*5, y*5, 5,5, colorArray);
                        //}
                        //ii += 2;
                        //d[index][3] = ii > 255 ? (byte)255 : (byte)ii;

                    //var ii = dS[index];
                    //if (ii < 256)
                    //{
                    //    ii += 2;
                    //    dS[index] = ii > 255 ? 255 : ii;
                    //}

                }
            }
        }

        for(var k = 0; k < 0;k++)
        {
            var f = (int)d[k][0];
            byte r = 0;
            byte g = 0;
            byte b = 0;
            if (f > 9&&f<=50)
            {
                r = 100;
                g = 100;
                b = 100;
            }
            else if (f > 50 && f <= 100)
            {
                r = 100;
                g = 150;
                b = 100;
            }
            else if (f > 100 && f <= 150)
            {
                r = 100;
                g = 150;
                b = 100;
            }
            else if (f > 150 && f <= 200)
            {
                r = 250;
                g = 150;
                b = 100;
            }
            else if (f > 200)
            {
                r = 255;
                g = 100;
                b = 100;
            }
            d[k][0] = r;
            d[k][1] = g;
            d[k][2] = b;
            d[k][3] = 255;

        }
        var bb = (int)(w / w2);
        for(int k = 0; k < w2; k++)
        {
            for(int j = 0; j < w2; j++)
            {

                    Color[] colorArray = new Color[bb*bb];

                    // fill this with our color
                    for (int pk = 0; pk < bb*bb; pk++)
                    {
                        var f = (int)ok[j+(k*w2)];
                        //Color32 c;
                        //c.b = (byte)((f) & 0xFF);
                        //c.g = (byte)((f >> 8) & 0xFF);
                        //c.r = (byte)((f >> 16) & 0xFF);
                        //c.a = (byte)((f >> 24) & 0xFF);

                    if (f > 9 && f <= 80)
                        {
                            colorArray[pk] = new Color(0, f / 255f, 0);
                        }
                        else if (f > 80 && f <= 120)
                        {
                            colorArray[pk] = new Color(0, 0, f / 255f);
                        }
                        else if (f > 120 && f <= 180)
                        {
                            colorArray[pk] = new Color(f / 255f, f / 255f, 0);
                        }
                        else if (f > 180)
                        {
                            colorArray[pk] = new Color(f / 255f,0, 0);
                        }
                       }
                    t.SetPixels(j*bb, k * bb, bb, bb, colorArray);
            }
        }

        //t.SetPixels32(d);
        t.Apply();
        image.texture = t;
    }
}
