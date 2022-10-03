using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_TriggerGround : MonoBehaviour
{
    public static LYJ_TriggerGround Instance;

    public bool isInsideLine;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isInsideLine = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isInsideLine = false;
        }
    }
}
