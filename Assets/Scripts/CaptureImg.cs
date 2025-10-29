using System.IO;
using UnityEngine;

public class CaptureImg : MonoBehaviour
{
    public int captureWidth = 1920; // Adjust the capture width as needed
    public int captureHeight = 1080; // Adjust the capture height as needed
    public int offset = 10000;
    public int width = 1;
    public int height = 1;
    public string PhotoName = "Name.png";
    public string savePath = "Assets/SavedImages/"; // Set the desired save path
    public Camera camera;
    private void Start()
    {
        
    }
    void Update()
    {
        // Example: Capture photo when a key is pressed (you can customize the condition)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    camera.transform.localPosition = new Vector3(j*offset,i*offset);
                    CaptureAndSavePhoto(i.ToString()+j.ToString()+PhotoName);
                }
            }
        }
    }

    void CaptureAndSavePhoto(string name)
    {
        // Create a RenderTexture to capture the screen
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        // Render the camera to the RenderTexture
        camera.Render(); 
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Encode the screenshot to a PNG file
        byte[] bytes = screenshot.EncodeToPNG();

        // Save the PNG file
        File.WriteAllBytes(savePath + name+".png", bytes);

        Debug.Log("Photo saved at: " + savePath+name);
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = camera.transform.position;
        quad.transform.localScale = new Vector3(offset, offset, offset);
        quad.transform.localEulerAngles = new Vector3(90f, 0, 0);
        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = screenshot;  
        meshRenderer.material = material;
#if UNITY_EDITOR
        string materialPath = Path.Combine(savePath, name);
        UnityEditor.AssetDatabase.CreateAsset(material, materialPath+"Material.mat");
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Material saved at: {materialPath}");
#endif
    }
}
