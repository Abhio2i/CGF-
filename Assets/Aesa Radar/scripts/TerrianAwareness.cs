using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrianAwareness : MonoBehaviour
{
    public Image target;
    public RenderTexture renderTexture;
    private Texture2D texture;

    void Start()
    {
        // Create a Texture2D with the same dimensions as the RenderTexture
        texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Initialize the UI image
        target.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        // Update transform position and rotation
        transform.position = new Vector3(transform.position.x, transform.root.position.y + 4800, transform.position.z);
        transform.eulerAngles = new Vector3(90f, transform.root.eulerAngles.y, 0f);

        // Copy RenderTexture to Texture2D
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;

        // Manipulate the texture
        ManipulateTexture(texture);

        // Apply the modified texture (target.sprite references the same texture)
        texture.Apply();
    }

    void ManipulateTexture(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            // Example manipulation: Color threshold
            if (pixels[i].r > 0.05f) // Adjust threshold as needed (normalized between 0 and 1)
            {
                pixels[i] = new Color(1f, 0.5f, 0.5f); // Light red
            }
            else
            {
                pixels[i] = new Color(0.5f, 1f, 0.5f); // Light green
            }
        }

        texture.SetPixels(pixels);
    }
}