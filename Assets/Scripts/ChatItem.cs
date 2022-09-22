using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* 채팅하기 */
// 1 텍스트 세팅
// 2 height 맞춰주기
public class ChatItem : MonoBehaviour
{
    private XRText chatText;
    private RectTransform rt;
    
    // Start is called before the first frame update
    void Awake()
    {
        chatText = GetComponent<XRText>();
        chatText.onChangeSize = OnChangedHeight;
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*// 1 preferredHeight 가 변경이 되면
        if (rt.sizeDelta.y != chatText.preferredHeight)
        {
            // 2 height 맞춰주기
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
        }*/
    }

    public void SetText(string chat)
    {
        // 1 텍스트 세팅
        chatText.text = chat;
    }

    void OnChangedHeight()
    {
        // 1 preferredHeight 가 변경이 되면
        if (rt.sizeDelta.y != chatText.preferredHeight)
        {
            // 2 height 맞춰주기
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
        }
    }
}
