using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloomIntensityController : MonoBehaviour
{
    public float targetIntensity = 1f; // The target bloom intensity you want to set
    public float contrast = 1f;
    private Bloom bloomSettings;
    private ColorGrading colorGrading;
    private PostProcessVolume postProcessVolume;

    private void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();

        // Check if the post-process volume has the Bloom effect
        if (postProcessVolume.profile.TryGetSettings(out bloomSettings))
        {
            // Modify the bloom intensity
            bloomSettings.intensity.value = targetIntensity;
        }

        // Check if the post-process volume has the Bloom effect
        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            // Modify the bloom intensity
            colorGrading.postExposure.value = contrast;
        }
        else
        {
            Debug.Log("not found");
        }
    }

    private void Update()
    {
        // You can change the target intensity over time if needed
        bloomSettings.intensity.value = Mathf.Lerp(bloomSettings.intensity.value, targetIntensity, Time.deltaTime);
        colorGrading.postExposure.value = contrast;
    }
}
