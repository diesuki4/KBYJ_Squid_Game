using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_EndLineTrigger : MonoBehaviour
{
    public GameObject player;
    public LYJ_MGGameManager mgGameManager;
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
        if (other.gameObject == player && isCounted == false && mgGameManager.isTargeted == false)
        {
            mgGameManager.state = LYJ_MGGameManager.State.End;
            isCounted = true;
        }
    }
}
