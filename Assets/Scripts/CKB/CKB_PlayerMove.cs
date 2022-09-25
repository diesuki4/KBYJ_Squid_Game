using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CKB_PlayerMove : MonoBehaviourPun
{
    [Header("이동 속도")]
    public float moveSpeed;
    [Header("점프력")]
    public float jumpPower;
    [Header("중력 배수")]
    public float gravityFactor;

    float gravity = 9.81f;
    float yVelocity;
    bool isJumping;

    CharacterController cc;
    CKB_Player player;

    private Animator anim;  //+(예지) animator 추가
    public bool onClimbing;

    void Start()
    {
        //+(예지) animator 추가
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        player = GetComponent<CKB_Player>();
    }

    void Update()
    {
        if (CKB_GameManager.Instance.photonMode)
            if (!photonView.IsMine)
                return;

        if (player.state != CKB_Player.State.Alive)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        //+(예지) animator 추가
        anim.SetFloat("Speed", v);
        anim.SetFloat("Direction", h);

        Vector3 vec = new Vector3(h, 0, v);

        // 걷기 애니메이션에 vec.magnitude 사용

        float hRaw = Input.GetAxisRaw("Horizontal");
        float vRaw = Input.GetAxisRaw("Vertical");
        Vector3 dir = transform.TransformDirection(new Vector3(hRaw, 0, vRaw)).normalized;

        if (cc.isGrounded)
        {
            yVelocity = 0;
            isJumping = false;
            anim.SetTrigger("Move");
        }
        else
        {
            yVelocity -= gravity * gravityFactor * Time.deltaTime;
        }

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
            anim.SetTrigger("Jump");
        }

        if (onClimbing)
        {
            // print("onClimbing");
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                anim.SetTrigger("Ladder");
                v = 0;
                yVelocity += 0.1f;
            }
            
        }

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
