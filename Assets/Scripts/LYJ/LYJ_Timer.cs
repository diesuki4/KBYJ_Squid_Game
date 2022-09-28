using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LYJ_Timer : MonoBehaviourPun, IPunObservable
{
    public float timeValue = 5;
    public TextMeshProUGUI timerText;
    public string sceneName;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                timeValue -= Time.deltaTime; // 프레임 == 60프레임에 1초 => 
            }
        }
        else
        {
            timeValue += 150;

            if (sceneName.Contains("Mugunghwa"))
            {
                // State.End로 변환
                LYJ_YeongHeeState lyjState = LYJ_YeongHee.Instance.GetComponent<LYJ_YeongHeeState>();
                lyjState.state = LYJ_YeongHeeState.State.End;
            }
            else if (sceneName.Contains("Waiting"))
            {
                // ☆ 3 00:00이 되면 방장이 다른 참가자들의 RpcLevel함수를 실행시킨다
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.LoadLevel("Mugunghwa");
            }
            // ++ 시간내 못끝냈을 때 보내는 거
        }
        
        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        // Debug.Log((int)timeValue);
        timerText.text = string.Format("{000}", (int)timeValue);
    }

    // ☆ 2 방장의 시간만 업데이트 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timeValue);
        }
        else
        {
            timeValue = (float)stream.ReceiveNext();
        }
    }
}
