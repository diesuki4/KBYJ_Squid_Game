using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_TriggerGround : MonoBehaviour
{
    public static LYJ_TriggerGround Instance;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.GetComponent<LYJ_MGPlayerInsideline>().insideLine = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.GetComponent<LYJ_MGPlayerInsideline>().insideLine = false;
        }
    }
}
