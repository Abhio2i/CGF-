using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class WeatherMode : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Image targetImage;         // UI Image component.
    public bool weather = false;
    public Camera camera;
    public TMPro.TMP_Dropdown weatherType;
    [Range(0f, 1f)]
    public float Contrast = 1f;
    private Texture2D outputTexture;  // RenderTexture ka final texture.
    private Color[] pixels;          // Pixels after reading from RenderTexture.
    private Color[] processedPixels; // Processed pixels for UI.

    private Thread processingThread;  // Separate thread for pixel processing.
    private bool isProcessing = false; // Flag to check processing status.
    private Texture2D tempTexture;
    private int count = 0;
    void Awake()
    {
        if (renderTexture == null || targetImage == null)
        {
            Debug.LogError("RenderTexture ya Target Image assign nahi kiya gaya!");
            return;
        }
        tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        outputTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        tempTexture.filterMode = FilterMode.Point;
        tempTexture.mipMapBias = 0f;

        renderTexture.filterMode = FilterMode.Point;
        renderTexture.mipMapBias = 0f;
        outputTexture.filterMode = FilterMode.Point;
        outputTexture.mipMapBias = 0f;

    }

    
    public void weatherMode(bool v)
    {
        weather = v;
        if (weather)
        {
            StartProcessing();
        }
    }

    private void Update()
    {
        // Update transform position and rotation
        //camera.transform.position = new Vector3(transform.position.x, transform.root.position.y + 5000, transform.position.z);
        camera.transform.eulerAngles = new Vector3(90f, camera.transform.root.eulerAngles.y, 0f);
    }

    public void FixedUpdate()
    {
       
        // Agar background thread mein pixel processing complete ho gayi hai to, UI update karein.
        if (isProcessing && processedPixels != null && weather && count >50)
        {
            count = 0;
            ApplyTextureToImage();
            isProcessing = false;
            StartProcessing();
        }
        count++; 
    }

    // Background thread me pixel processing ko start karne ka method.
    public void StartProcessing()
    {
        if (!isProcessing)
        {
            ReadPixelsFromRenderTexture();
            // Run the background thread to process the texture pixels.
            processingThread = new Thread(ProcessPixelsInBackground);
            processingThread.Start();
        }
    }

    // Main thread me texture read karenge. 
    private void ReadPixelsFromRenderTexture()
    {
        // RenderTexture se data ko Texture2D me read karna.
        /*tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);*/
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();
        RenderTexture.active = null;

        // Get all pixels from the texture (main thread operation).
        pixels = tempTexture.GetPixels();

        // Cleanup the temporary texture.
        //Destroy(tempTexture);
    }

    private void ProcessPixelsInBackground()
    {
        // Pixel processing ko background thread me perform karte hain.
        processedPixels = new Color[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            float grayscale = pixels[i].grayscale; // Grayscale value (0 to 1).
            processedPixels[i] = MapGrayscaleToColor(grayscale);
        }
        isProcessing = true;
    }

    private void ApplyTextureToImage()
    {
        // Processed pixels ko texture me set karenge aur final texture ko UI Image ke Sprite me set karenge.
       
        outputTexture.SetPixels(processedPixels);
        outputTexture.Apply();

        Sprite sp = Sprite.Create(outputTexture, new Rect(0, 0, renderTexture.width, renderTexture.height), Vector2.one * 0.5f);
        targetImage.sprite = sp;
    }

    private Color MapGrayscaleToColor(float grayscale)
    {
        // Clamp contrast between 0 and 1 to avoid invalid values.
        Contrast = Mathf.Clamp01(Contrast);

        // Adjust grayscale based on contrast.
        grayscale = Mathf.Pow(grayscale, 1.0f / (Contrast + 0.01f)); // Higher contrast compresses low values.

        if (grayscale < 0.33f)
        {
            if (weatherType.value == 3)
            {
                // Black to Green.
                return Color.Lerp(Color.black, Color.yellow, grayscale / 0.33f);
            }
            else
            if (weatherType.value == 4)
            {
                // Black to Green.
                return Color.Lerp(Color.black, Color.yellow, grayscale / 0.33f);
            }
            // Black to Green.
            return Color.Lerp(Color.black, Color.green, grayscale / 0.33f);
        }
        else if (grayscale < 0.66f)
        {
            if (weatherType.value == 3)
            {
                // Black to Green.
                return Color.Lerp(Color.yellow, Color.red, (grayscale - 0.33f) / 0.33f);
            }
            else
            if (weatherType.value == 4)
            {
                // Black to Green.
                return Color.Lerp(Color.magenta, Color.magenta, (grayscale - 0.33f) / 0.33f);
            }
            // Green to Yellow.
            return Color.Lerp(Color.green, Color.yellow, (grayscale - 0.33f) / 0.33f);
        }
        else
        {
            if (weatherType.value == 3)
            {
                // Black to Green.
                return Color.Lerp(Color.red, Color.magenta, (grayscale - 0.66f) / 0.34f);
            }
            else
            if (weatherType.value == 4)
            {
                // Black to Green.
                return Color.Lerp(Color.magenta, Color.magenta, (grayscale - 0.66f) / 0.34f);
            }
            // Yellow to Red.
            return Color.Lerp(Color.yellow, Color.yellow, (grayscale - 0.66f) / 0.34f);
        }
    }

    private Color MapGrayscaleToColors(float grayscale)
    {
        // Map grayscale to color (0 -> Black, 1 -> Red with gradient).
        if (grayscale < 0.33f * Contrast)
        {
            // Black to Green.
            return Color.black;
        }
        else
        if (grayscale < 0.45f * Contrast)
        {
            // Black to Green.
            return Color.Lerp(Color.black, Color.green, grayscale / 0.33f);
        }
        else if (grayscale < 0.5f * (1-Contrast))
        {
            if (weatherType.value == 3)
            {
                // Black to Green.
                return Color.Lerp(Color.yellow, Color.red, (grayscale - 0.33f) / 0.33f);
            }
            else
            if (weatherType.value == 4)
            {
                // Black to Green.
                return Color.Lerp(Color.red, Color.magenta, (grayscale - 0.33f) / 0.33f);
            }
                // Green to Yellow.
                return Color.Lerp(Color.green, Color.yellow, (grayscale - 0.33f) / 0.33f);
        }
        else
        {
            if (weatherType.value == 3)
            {
                // Black to Green.
                return Color.Lerp(Color.red, Color.red, (grayscale - 0.66f) / 0.34f);
            }
            else
            if (weatherType.value == 4)
            {
                // Black to Green.
                return Color.Lerp(Color.magenta, Color.magenta, (grayscale - 0.66f) / 0.34f);
            }
            // Yellow to Red.
            return Color.Lerp(Color.yellow, Color.red, (grayscale - 0.66f) / 0.34f);
        }
    }

    private void OnDestroy()
    {
        // Ensure thread is stopped and memory is cleaned up.
        if (processingThread != null && processingThread.IsAlive)
        {
            processingThread.Abort(); // Forcefully stop the thread if it's still running.
        }

        if (outputTexture != null)
        {
            Destroy(outputTexture);
        }
    }
}
