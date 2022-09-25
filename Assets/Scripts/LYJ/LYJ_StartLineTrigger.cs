using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_StartLineTrigger : MonoBehaviour
{
    public Animator anim;
    public GameObject player;
    public bool onLadder;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<CKB_PlayerMove>().onClimbing = onLadder;
    }

    private void OnTriggerEnter(Collider other)
    {
        onLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        onLadder = false;
    }
}
