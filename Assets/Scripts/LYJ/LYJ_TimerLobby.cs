using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_TimerLobby : MonoBehaviour
{    
    public float timeValue = 100;

    #region LEDTimer import
    private string LedText;
    private LedBoardScript ledBoard;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ledBoard = GetComponent<LedBoardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime; // 프레임 == 60프레임에 1초 => 
        }
        else
        {
            timeValue += 150;
            // State.End로 변환
            LYJ_YeongHeeState lyjState = LYJ_YeongHee.Instance.GetComponent<LYJ_YeongHeeState>();
            lyjState.state = LYJ_YeongHeeState.State.End;
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
        ledBoard.isBlink = true;
        ledBoard.LedText = string.Format("{000}", (int)timeValue);
    }
}
