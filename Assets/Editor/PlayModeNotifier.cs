using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PlayModeNotifier
{
    static PlayModeNotifier()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // This code runs every time the play mode starts
            Debug.Log("Yay! The game is now playing!");
            // Add your custom code here
        }
    }
}
