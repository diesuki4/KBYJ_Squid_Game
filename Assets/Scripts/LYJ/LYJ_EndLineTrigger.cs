using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_EndLineTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject yeonghee;
    
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
        if (other.gameObject == player)
        {
            yeonghee.GetComponent<LYJ_YeongHeeState>().playerEndCount++;
        }
    }
}
