using System.Collections;
using UnityEngine;

public class FilePicker
{
    public enum Mode
    {
        File,
        Directory
    }

    public enum Result
    {
        Ok,
        Cancel
    }

    public Result result { get; private set; }
    public string path { get; private set; }

    private Mode mode;

    public FilePicker()
    {
        result = Result.Cancel;
    }

    public void SetMode(Mode mode)
    {
        this.mode = mode;
    }

    public IEnumerator Open()
    {
        yield return null; // Simulate opening a file picker dialog
        path = "/CGF_Mission_Feedback";
        result = Result.Ok;
    }
}
