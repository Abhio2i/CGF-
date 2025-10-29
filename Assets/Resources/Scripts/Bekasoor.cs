using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bekasoor : MonoBehaviour
{
    public string folderPath = "Assets/"; // Default path
    public string extension = ".cs";
    public string Target = "burahua.cs";
    // Start is called before the first frame update
    void Start()
    {
        ListFilesInFolder(folderPath);
    }

    private void ListFilesInFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("The specified folder path does not exist.");
            return;
        }

        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        Debug.Log($"Files in folder '{path}':");

        foreach (string file in files)
        {
            if(file.EndsWith(extension))
            Debug.Log(file);

            if (file.Contains(Target))
                ChangeLineInFile(file, 9, "Debug.Log(\" hello world \");");
        }
    }

    private void ChangeLineInFile(string path, int line, string content)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("The specified file path does not exist.");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        if (line < 0 || line >= lines.Length)
        {
            Debug.LogError("The specified line number is out of range.");
            return;
        }

        lines[line] = content;
        File.WriteAllLines(path, lines);

        Debug.Log($"Line {line + 1} in file '{path}' has been changed to: {content}");
    }
}
