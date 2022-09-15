using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_Bridge : MonoBehaviour
{
    public bool usingGravity;
    private Rigidbody rb;
    private BoxCollider bc;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        /*// 1 만약 usingGravity가 true이면
        if (usingGravity)
        {
            // 2 gravity 켜준다
            bc.isTrigger = true;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1 만약 usingGravity가 true이면
        if (usingGravity)
        {
            // 2 gravity 켜준다
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}