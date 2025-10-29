using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class FolderPickup : MonoBehaviour
{
    public Text folderPathText; // Optional: UI Text to display selected folder path

    // Coroutine to open folder picker dialog
    IEnumerator OpenFolderPickerCoroutine()
    {
        // Create a file picker object
        var picker = new FilePicker();
        picker.SetMode(FilePicker.Mode.Directory);

        // Show the file picker and wait for it to finish
        yield return StartCoroutine(picker.Open());

        // Check if a folder was selected
        if (picker.result != FilePicker.Result.Ok)
        {
            Debug.Log("Folder selection cancelled.");
            yield break;
        }

        // Get the selected folder path
        string folderPath = picker.path;

        // Display the selected folder path
        Debug.Log("Selected folder: " + folderPath);

        // Optionally, update UI with selected folder path
        if (folderPathText != null)
        {
            folderPathText.text = folderPath;
        }

        // Access folder contents if needed
        string[] files = Directory.GetFiles(folderPath);
        foreach (string file in files)
        {
            Debug.Log("File: " + file);
        }
    }

    // Method to start the folder picker coroutine
    public void OpenFolderPicker() 
    {
        StartCoroutine(OpenFolderPickerCoroutine());
    }
}
