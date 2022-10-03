using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_PlayerMoveDetect : MonoBehaviour
{
    public Vector3 lastPos;
    public Vector3 lastRot;
    public bool isMoving;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        CheckChangePosition();
        CheckChangeRotation();
        // Debug.Log("lastPos: " + lastPos);
        // Debug.Log("isMoving: " + isMoving);
    }

    public void CheckChangePosition()
    {
        if (lastPos != transform.position)
        {
            lastPos = transform.position;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
    public void CheckChangeRotation()
    {
        if (lastRot != transform.eulerAngles)
        {
            lastRot = transform.eulerAngles;
        }
    }

    public bool IsPlayerMoving()
    {
        if (isMoving)
            return true;
        else
            return false;
    }
}
