using System;
using UnityEngine;
using UnityEngine.UI;

public class ApplyStream : MonoBehaviour
{

    private Camera cam;
    public RawImage rawImage;
    public RenderTexture tex;
    public GameObject display;
    public bool rbm = false;
    // Use this for initialization
    public Shader shader;
    [Range(0f, 10f)]
    public float depthLevel = 0.5f;
    public Texture2D t;



    private Material _material;
    private Material material
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
    }

    private RenderBuffer colorBuffer;
    private RenderBuffer depthBuffer;

    void Start()
    {

        t = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false); 
        cam = GetComponent<Camera>();

        //cam.SetTargetBuffers(colorBuffer, tex.depthBuffer);
        cam.SetTargetBuffers(tex.colorBuffer, tex.depthBuffer);

        //display.transform.localScale = new Vector3(cam.pixelWidth/ 10, cam.pixelHeight/10, 1);



        if (!SystemInfo.supportsImageEffects)
        {
            print("System doesn't support image effects");
            enabled = false;
            return;
        }
        if (shader == null || !shader.isSupported)
        {
            enabled = false;
            print("Shader " + shader.name + " is not supported");
            return;
        }

        // turn on depth rendering for the camera so that the shader can access it via _CameraDepthTexture
        cam.depthTextureMode = DepthTextureMode.Depth;
        //Invoke("update", 0.5f);
    }
    //private void Update()
    //{
    //    //transform.parent.position = transform.parent.parent.position + new Vector3(0, 500, 0);
    //    //transform.parent.rotation = Quaternion.Euler(0, transform.parent.parent.eulerAngles.y, 0);
    //}
    // Update is called once per frame
    void Update()
    {

        material.SetFloat("_DepthLevel", depthLevel);

        RenderTexture.active = tex;
        cam.Render();
        Graphics.Blit(tex, tex, material);


        //Texture2D t = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        //Texture2D t = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        // Read pixels from screen into the saved texture data.
        t.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        // Actually apply all previous SetPixel and SetPixels changes.
        //t.Apply();    // Unnecessary? YES Necessary!!!
        var d = t.GetPixels32();
        //Debug.Log(d[400*199+200]);
        for (var i = 0; i < d.Length; i++)
        {
            var val = d[i][0];

            //d[i][0] = (byte)(255- (int)val);
            //d[i][1] = 0;
            //d[i][2] = val;
            d[i][3] = 255;
            var gr = (int)d[i][0];
            if (gr >= 230)
            {
                var s = Mathf.Round(((255 - gr) / 19f) * 255);
                d[i][0] = 0;
                d[i][1] = 0;
                d[i][2] = (byte)s;
            }
            else if (gr < 230 && gr > 180)
            {
                var s = Mathf.Round(((230 - gr) / 50f) * 155);
                d[i][0] = 0;
                d[i][1] = (byte)(100 + s);
                d[i][2] = 0;
            }
            else if (gr <= 180 && gr > 100)
            {
                var s = Mathf.Round(((180 - gr) / 80f) * 155);
                d[i][0] = (byte)(100 + s);
                d[i][1] = (byte)(100 + s);
                d[i][2] = 0;
            }
            else if (gr <= 100)
            {
                var s = Mathf.Round(((100 - gr) / 100f) * 155);
                d[i][0] = (byte)(100 + s);
                d[i][1] = 0;
                d[i][2] = 0;
            }
        }
        //var vs = cam.ScreenToWorldPoint(new Vector3(Mathf.Floor((400 * 199 + 200) / 400f), (400 * 199 + 200) / 400, d[400 * 199 + 200][0]*117));
        //var dis = vs.y - transform.position.y;
        //dis = dis < 0 ? -dis : dis;
        //dis = dis > 1000 ? 1000 : dis;
        //dis = Mathf.Floor(dis / 3.92f);
        //Debug.Log(dis);

        if (rbm)
        {
            

            ///////////////////////////
            for (var i = 0; i < d.Length; i++)
            {
                var val = d[i][0];
                var v = cam.ScreenToWorldPoint(new Vector3(Mathf.Floor(i / 400f), i / 400, val * 117));
                var di = transform.position.y - v.y;
                di = di < 0 ? 0 : di;
                di = di > 1000 ? 1000 : di;
                di = val == 255 ? 1000 : di;
                di = Mathf.Floor(di / 3.92f);
                //d[i][0] = (byte)(255- (int)val);
                //d[i][1] = 0;
                //d[i][2] = val;
                d[i][3] = 255;
                var gr = di;// (int)d[i][0];
                            //if (gr<240&&gr >= 230)
                            //{
                            //    var s = Mathf.Round(((255 - gr) / 19f) * 255);
                            //    d[i][0] = 0;
                            //    d[i][1] = 0;
                            //    d[i][2] = (byte)s;
                            //}
                if (gr < 255 && gr > 180)
                {
                    var s = Mathf.Round(((230 - gr) / 85f) * 155);
                    d[i][0] = 0;
                    d[i][1] = (byte)(100 + s);
                    d[i][2] = 0;
                }
                else if (gr <= 180 && gr > 100)
                {
                    var s = Mathf.Round(((180 - gr) / 80f) * 155);
                    d[i][0] = (byte)(100 + s);
                    d[i][1] = (byte)(100 + s);
                    d[i][2] = 0;
                }
                else if (gr <= 100)
                {
                    var s = Mathf.Round(((100 - gr) / 100f) * 155);
                    d[i][0] = (byte)(100 + s);
                    d[i][1] = 0;
                    d[i][2] = 0;
                }
            }
            t.SetPixels32(d);
            
        }
        //t.Apply();
        /*
        var d2 = t.GetPixels32();
        //Debug.Log(d2.Length);
        
        for (var i = 0; i < d.Length; i++)
        {
            var val = d[i][0];
            var v = cam.ScreenToWorldPoint(new Vector3(Mathf.Floor(i / 400f), i / 400, val * 117));
            var di = transform.position.y - v.y;
            di = di < 0 ? 0 : di;
            di = di > 5000 ? 5000 : di;
            di = val == 255 ? 5000 : di;
            di = Mathf.Floor(di / 3.92f);
            //d[i][0] = (byte)(255- (int)val);
            //d[i][1] = 0;
            //d[i][2] = val;
            d[i][3] = 255;
            var gr = di;// (int)d[i][0];
            //if (gr<240&&gr >= 230)
            //{
            //    var s = Mathf.Round(((255 - gr) / 19f) * 255);
            //    d[i][0] = 0;
            //    d[i][1] = 0;
            //    d[i][2] = (byte)s;
            //}
            if (gr < 255 && gr > 180)
            {
                var s = Mathf.Round(((230 - gr) / 85f) * 155);
                d[i][0] = 0;
                d[i][1] = (byte)(100 + s);
                d[i][2] = 0;
            }
            else if (gr <= 180 && gr > 50)
            {
                var s = Mathf.Round(((180 - gr) / 130f) * 155);
                d[i][0] = (byte)(100 + s);
                d[i][1] = (byte)(100 + s);
                d[i][2] = 0;
            }
            else if (gr <= 50)
            {
                var s = Mathf.Round(((50 - gr) / 50f) * 155);
                d[i][0] = (byte)(100 + s);
                d[i][1] = 0;
                d[i][2] = 0;
            }

            var x = new Vector3(v.x, 0, v.z) - new Vector3(transform.position.x, 0, transform.position.z);
            x = Quaternion.Inverse(Quaternion.Euler(0, transform.eulerAngles.y, 0)) * x;
            var xx = (int)Mathf.Floor(x.x / 150) + 200;
            xx = xx > 399 ? 400 : xx;
            var yy = (int)Mathf.Floor(x.z / 75);
            yy = yy > 399 ? 400 : yy;
            d2[xx * yy][0] = d[i][0];
            d2[xx * yy][1] = d[i][1];
            d2[xx * yy][2] = d[i][2];
        }
        */
        // t.SetPixels32(d);
        t.Apply();
        //if(display)
        //display.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = t;
        rawImage.material.mainTexture = t;
        //rawImage.texture = t;
        RenderTexture.active = null;
        //Invoke("update", 0.5f);
    }
}
