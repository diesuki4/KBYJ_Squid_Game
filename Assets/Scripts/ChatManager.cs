using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* chatting 창 만들기 */
// 1 글을 쓰다가 엔터를 치면
// 2 ChatItem 을 하나 만든다 - 부모를 ScrollView/Content
// 3 text 컴포넌트를 가져와서 inputField 내용을 세팅

/* chatting 창 세부 조정 */
// 1 채팅을 쓸 때도 플레이어 움직임 막기
// 2 닉네임 : 채팅 내용
// 3 채팅 내용 많으면 개행
// 4 채팅창 끝을 보고 있을 때 채팅 추가 시 스크롤 위치 변경
public class ChatManager : MonoBehaviourPun
{
    public InputField inputChat;
    public Text chatItemFactory;

    private Text chattingText;
    private Color nickColor;

    public RectTransform trContent;
    public 

    // Start is called before the first frame update
    void Start()
    {
        // inputField Enter => 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);

        /* chatting 창 세부 조정 */
        // 1 채팅을 쓸 때도 플레이어 움직임 막기
        // 1-1 커서를 안보이게 
        nickColor = new Color(
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f)
        );
    }

    // Update is called once per frame
    void Update()
    {
        inputChat.ActivateInputField();
    }


    private void OnValueChanged(string s)
    {
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    private void OnSubmit(string s)
    {
        // <color=#FF0000>닉네임 </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;
        
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);

        inputChat.text = "";
        inputChat.ActivateInputField();
    }

    public RectTransform rtScrollView;
    private float prevContentH;

    [PunRPC]
    private void RpcAddChat(string chat)
    {
        // 이전 content의 H값을 저장한다
        prevContentH = trContent.sizeDelta.y;
        
        // 2 ChatItem 을 하나 만든다
        // (부모를 ScrollView - content)
        // chattingText = Instantiate(chatItemFactory, contentPos.transform.position, contentPos.transform.rotation, contentPos.transform);
        print("chatItemFactory: " + chatItemFactory);
        chattingText = Instantiate(chatItemFactory, trContent);

        // 3 text 컴포넌트 가져와서 inputField 의 내용을 세팅
        ChatItem chatItem = chattingText.GetComponent<ChatItem>();
        chatItem.SetText(chat);

        StartCoroutine("AutoScrollBottom");
    }

    IEnumerator AutoScrollBottom()
    {
        // 양보 후 다음 프레임에 실행
        // => 크기 변경후 content 위치 변경
        yield return null;
        
        // 스크롤뷰 H보다 content H값이 클 때만 => 스크롤이 가능한 상태
        if (trContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {
            // 4 이전에 바닥에 닿았다면 (contnet y >= 변경되기 전 content H - current 스크롤뷰 H)
            // 현재 해상도 좌표
            // anchoredPosition = UI anchor 기준
            if (trContent.anchoredPosition.y >= prevContentH - rtScrollView.sizeDelta.y)
            {
                // 5 추가된 높이만큼 content y값을 변경하겠다
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - rtScrollView.sizeDelta.y);
            }
        }
    }
}