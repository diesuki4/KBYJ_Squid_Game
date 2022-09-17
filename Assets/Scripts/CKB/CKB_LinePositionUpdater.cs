using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_LinePositionUpdater : MonoBehaviour
{
    LineRenderer lineRenderer;

    Vector3 prevPos;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        prevPos = transform.position;
    }

    void Update()
    {
        if (transform.position == prevPos)
            return;

        for (int i = 0; i < lineRenderer.positionCount; ++i)
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + (transform.position - prevPos));

        prevPos = transform.position;
    }
}
