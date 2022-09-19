using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CKB_PlayerRotate : MonoBehaviourPun
{
    [Header("카메라 X축 회전 속도")]
    public float rotSpeedX;
    [Header("카메라 Y축 회전 속도")]
    public float rotSpeedY;
    [Header("카메라 X축 최대 회전각")]
    public float maxRotX;

    Transform mainCamBasePos;

    float rotX;
    float rotY;
    float _minRotX;
    float _maxRotX;

    CKB_Player player;

    void Start()
    {
        if (!CKB_GameManager.Instance.photonMode || photonView.IsMine)
        {
            Transform mainCam = transform.Find("Main Camera");
            mainCam.gameObject.SetActive(true);

            mainCamBasePos = transform.Find("Main Camera Base Position");

            mainCam.SetParent(mainCamBasePos.Find("Main Camera Position"));
            mainCam.localEulerAngles = mainCam.localPosition = Vector3.zero;

            rotX = -mainCamBasePos.localEulerAngles.x;
            _minRotX = rotX - maxRotX;
            _maxRotX = rotX + maxRotX;
        }
        
        player = GetComponent<CKB_Player>();
    }

    void Update()
    {
        if (CKB_GameManager.Instance.photonMode)
            if (!photonView.IsMine)
                return;
        
        if (player.state != CKB_Player.State.Alive)
            return;

        rotY += Input.GetAxis("Mouse X") * rotSpeedY * Time.deltaTime;
        rotX += Input.GetAxis("Mouse Y") * rotSpeedX * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, _minRotX, _maxRotX);

        transform.eulerAngles = new Vector3(0, rotY, 0);

        Vector3 mainCamAngles = mainCamBasePos.eulerAngles;
        mainCamBasePos.eulerAngles = new Vector3(-rotX, mainCamAngles.y, mainCamAngles.z);
    }
}
