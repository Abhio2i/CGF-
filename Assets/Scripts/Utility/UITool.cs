using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITool : MonoBehaviour
{
    static GameObject toolTipUI;
    static Coroutine toolTipUIdestroyCoroutine;

    public static void ToolTip(string text)
    {
        return;
        if (toolTipUIdestroyCoroutine != null)
        {
            UITool instance = FindObjectOfType<UITool>(); // Get an instance of UITool
            if (instance != null)
            {
                instance.StopCoroutine(toolTipUIdestroyCoroutine);
            }
            toolTipUIdestroyCoroutine = null;
        }

        if (toolTipUI == null)
        {
            // Load the prefab from the Resources folder
            GameObject prefab = Resources.Load<GameObject>("ToolTip");
            if (prefab != null)
            {
                // Instantiate the prefab in the scene
                toolTipUI = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                
                Text tex = toolTipUI.GetComponentInChildren<Text>();
                tex.text = text;
                // Get mouse position in screen space
                Vector2 mousePosition = Input.mousePosition;

                // Convert screen space to canvas space
                Vector2 anchoredPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    toolTipUI.GetComponent<RectTransform>(),
                    mousePosition,
                    null, // Camera is null for Screen Space - Overlay
                    out anchoredPosition
                );

                // Update the position of the tooltip
                tex.transform.parent.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
                Debug.Log("Prefab loaded and instantiated!");
            }
            else
            {
                Debug.LogError("Prefab not found in Resources folder!");
                return;
            }
        }
        else
        {
            // Update the text
            Text tex = toolTipUI.GetComponentInChildren<Text>();
            tex.text = text;
            // Get mouse position in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Convert screen space to canvas space
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                toolTipUI.GetComponent<RectTransform>(),
                mousePosition,
                null, // Camera is null for Screen Space - Overlay
                out anchoredPosition
            );

            // Update the position of the tooltip
            tex.transform.parent.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }

        // Schedule destruction in 5 seconds
        UITool instanceMono = FindObjectOfType<UITool>(); // Get an instance of UITool
        if (instanceMono != null)
        {
            toolTipUIdestroyCoroutine = instanceMono.StartCoroutine(toolTipScheduleDestroy(0.5f));
        }
    }

    static IEnumerator toolTipScheduleDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (toolTipUI != null)
        {
            Destroy(toolTipUI);
            toolTipUI = null;
            Debug.Log("Tooltip destroyed!");
        }
    }
}
