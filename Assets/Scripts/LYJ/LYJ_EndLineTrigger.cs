using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_EndLineTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject yeonghee;
    private bool isCounted;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && isCounted == false)
        {
            yeonghee.GetComponent<LYJ_YeongHeeState>().playerEndCount++;
            isCounted = true;
        }
    }
}
