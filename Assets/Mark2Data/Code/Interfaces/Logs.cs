using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Logs : MonoBehaviour
{
    PointInTime pointInTime;
    List<string> messages;

    [HideInInspector] public List<Vector3> positions;


    [SerializeField] GameObject scrollContent;
    [SerializeField] GameObject text;

    TMP_Text textArea;
    private void Awake()
    {
        pointInTime = new PointInTime();

        messages = pointInTime.GetMessages();
        positions = pointInTime.Positions();
        foreach (string message in messages)
        {
            AssignText(message);
        }
    }

    void AssignText(string message) // creating messages
    {
        GameObject _tempText=Instantiate(text);
        _tempText.transform.SetParent(scrollContent.transform, false);
        textArea=text.GetComponent<TMP_Text>();
        textArea.text=message;
    }

    void FilterPositions(Vector3 positions)
    {

    }


}
