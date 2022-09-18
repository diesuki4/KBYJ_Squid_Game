using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_LinePositionUpdater : MonoBehaviour
{
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; ++i)
            lineRenderer.SetPosition(i, transform.GetChild(i).position);        
    }

    public Transform ClosestBone(Vector3 pos)
    {
        Transform closestBone = null;
        float min = float.MaxValue;

        foreach (Transform tr in transform)
        {
            float distance = Vector3.Distance(tr.position, pos);

            if (distance < min)
            {
                closestBone = tr;
                min = distance;
            }
        }

        return closestBone;
    }
}
