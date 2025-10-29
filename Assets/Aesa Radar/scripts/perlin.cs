using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perlin : MonoBehaviour
{
    public ParticleSystem m_System;
    public ParticleSystem.Particle[] m_Particles;


    public GameObject prefab;
    public float scal = 1f;
    public bool genrate = false;
    public List<GameObject> l = new List<GameObject>();

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
        // Set up the texture and a Color array to hold pixels during processing.

    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                //Debug.Log(sample);
                if (sample > 0.4f)
                {
                    var dd = Mathf.Ceil(sample * 30) - 12f;
                    for (var d = 0; d < dd; d++)
                    {
                        var obj = Instantiate(prefab,Vector3.zero,Quaternion.identity);
                        obj.transform.position = new Vector3((int)x * scal, (((-dd/2f)+d)*scal)+transform.position.y, (int)y * scal);
                        l.Add(obj);
                    }

                }

                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }


    void Update()
    {
        if (genrate)
        {
            //noiseTex = new Texture2D(pixWidth, pixHeight);
            //pix = new Color[noiseTex.width * noiseTex.height];
            //rend.material.mainTexture = noiseTex;
            //genrate = false;
            //foreach (var ob in l)
            //{
            //    Destroy(ob.gameObject);
            //}
            //CalcNoise();
            int numParticlesAlive = m_System.GetParticles(m_Particles);
            
        }
    }
}
