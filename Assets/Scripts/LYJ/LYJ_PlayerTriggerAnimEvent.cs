using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_PlayerTriggerAnimEvent : MonoBehaviour
{
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
        if (other.gameObject.name.Contains("Player"))
        {
            print("Lerp 사용 예정");
            
            // master
            
            
            // other
        }
    }
}
