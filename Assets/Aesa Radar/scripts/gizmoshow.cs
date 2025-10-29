using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class gizmoshow : MonoBehaviour
{
    public enum Colr{
        red,
        blue,
        green,
        yellow,
        pink
    }
    public bool gizmo = false;
    public List<float> size = new List<float>();
    public Camera cm;
    public bool select=false;
    //public Colr color = Colr.red;
    public Color colr = Color.red;
    public Color selectColor = Color.blue;
    //public GameObject obj;
    //public GameObject red;
    //public GameObject blue;
    //public GameObject green;
    //public GameObject yellow;
    //public GameObject pink;

    static Material lineMaterial;

    void Start()
    {
        //switch (color)
        //{
        //    case Colr.red:
        //        obj = Instantiate(red);
        //        break;
        //    case Colr.blue:
        //        obj = Instantiate(blue);
        //        break;
        //    case Colr.green:
        //        obj = Instantiate(green);
        //        break;
        //    case Colr.yellow:
        //        obj = Instantiate(yellow);
        //        break;
        //    case Colr.pink:
        //        obj = Instantiate(pink);
        //        break;
        //}


        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 2);
        Camera.onPostRender += onPostRender;

    }

    //private void FixedUpdate()
    //{
    //    //obj.transform.localScale = new Vector3(gizmoSize, gizmoSize, gizmoSize);
    //    //obj.transform.position = transform.position;
    //    //obj.transform.rotation = transform.rotation;
    //    //Debug.Log(color.ToString());
    //}

    private void OnDestroy()
    {
        Camera.onPostRender -= onPostRender;
    }

    void onPostRender(Camera cam)
    {
        if (cm != null)
        {
            if(cm != cam)
            {
                return;
            }
        }

        if (lineMaterial)
        {
            lineMaterial.SetPass(0);
        }
        else
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 2);
        }

        foreach (float gizmoSize in size)
        {
            var pos = transform.position;
            var posx = transform.position + transform.forward * (gizmoSize + (gizmoSize / 2));
            var posz = transform.position;// transform.position - transform.forward * (gizmoSize + (gizmoSize / 2));

            GL.Begin(GL.LINES);
            GL.Color(select?selectColor:colr);
            
            GL.Vertex(posx);
            GL.Vertex(posz);
            posx = transform.position + transform.forward * (gizmoSize );
            posz = transform.position + (transform.right * (gizmoSize )) - (transform.forward * (gizmoSize));
            GL.Vertex(posx);
            GL.Vertex(posz);

            //posx = transform.position + transform.forward * (gizmoSize + (gizmoSize / 2));
            posz = transform.position - (transform.right * (gizmoSize)) - (transform.forward * (gizmoSize));
            GL.Vertex(posx);
            GL.Vertex(posz);
            posx = transform.position + (transform.right * (gizmoSize)) - (transform.forward * (gizmoSize));
            posz = transform.position - (transform.right * (gizmoSize)) - (transform.forward * (gizmoSize));
            GL.Vertex(posx);
            GL.Vertex(posz);


            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));

            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));

            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x + gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z + gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y + gizmoSize, pos.z - gizmoSize));
            GL.Vertex(new Vector3(pos.x - gizmoSize, pos.y - gizmoSize, pos.z - gizmoSize));

            

            GL.End();
        }
        //Debug.Log(transform.position);


    }

    private void OnDrawGizmos()
    {
        onPostRender(Camera.main);
    }
}
