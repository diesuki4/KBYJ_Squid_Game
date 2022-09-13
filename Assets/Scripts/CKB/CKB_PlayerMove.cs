using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_PlayerMove : MonoBehaviour
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

    void Start()
    {
        cc = GetComponent<CharacterController>();
        player = GetComponent<CKB_Player>();
    }

    void Update()
    {
        if (player.state != CKB_Player.State.Alive)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vec = new Vector3(h, 0, v);

        // 걷기 애니메이션에 vec.magnitude 사용

        float hRaw = Input.GetAxisRaw("Horizontal");
        float vRaw = Input.GetAxisRaw("Vertical");
        Vector3 dir = transform.TransformDirection(new Vector3(hRaw, 0, vRaw)).normalized;

        if (cc.isGrounded)
        {
            yVelocity = 0;
            isJumping = false;
        }
        else
        {
            yVelocity -= gravity * gravityFactor * Time.deltaTime;
        }

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
