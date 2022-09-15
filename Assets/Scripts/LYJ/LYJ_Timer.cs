using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LYJ_Timer : MonoBehaviour
{
    public float timeValue = 5;
    public TextMeshProUGUI timerText;
    
    // Start is called before the first frame update
    void Start()
    {
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
        timerText.text = string.Format("{000}", (int)timeValue);
    }
}
