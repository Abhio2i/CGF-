using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjust : MonoBehaviour
{
    public RectTransform parent;
    public RectTransform targetParent;
    public List<RectTransform> child;
    public float Offset = 0;
    public float Space = 0;
    void Start()
    {
        child = new List<RectTransform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            child.Add(parent.GetChild(i).GetComponent<RectTransform>());
        }
        if(targetParent == null)
        {
            targetParent = parent;
        }
    }

    public void adjustHeight()
    {
        if (targetParent == null)
        {
            targetParent = parent;
        }
        float h = 0f;
        foreach (RectTransform c in child)
        {
            if (c.gameObject.activeSelf)
            {
                h += c.sizeDelta.y;
            }
        }
        h += (child.Count * Space) + Offset;
        targetParent.sizeDelta = new Vector2(targetParent.sizeDelta.x, h);
        //Debug.Log(h);
    }
}
