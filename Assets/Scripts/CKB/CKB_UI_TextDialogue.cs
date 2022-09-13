using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

class ConversationEntity
{
    public bool isEnd = false;
    public bool isPlaying = false;
    public string text;
    public float wordTerm;
    public float endDelay;
    public int fontSize;
    public AudioClip audioClip;
}

public class CKB_UI_TextDialogue : MonoBehaviour
{
    public static CKB_UI_TextDialogue Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    [HideInInspector]
    public Action onStart;
    [HideInInspector]
    public DG.Tweening.TweenCallback onComplete;

    RectTransform dialoguePanel;
    Text conversationText;

    Queue<ConversationEntity> conversationQueue;
    bool isAppearingDialogue;

    void Start()
    {
        dialoguePanel = transform.Find("Bottom Center Alignment").GetComponent<RectTransform>();
        conversationText = dialoguePanel.Find("Conversation Text").GetComponent<Text>();

        dialoguePanel.anchoredPosition = Vector2.down * 300;

        conversationQueue = new Queue<ConversationEntity>();
    }

    void Update()
    {
        ConversationEntity convEntity = null;

        if (!isAppearingDialogue)
            if (conversationQueue.TryPeek(out convEntity))
                if (convEntity.isEnd)
                    conversationQueue.Dequeue();
                else if (!convEntity.isPlaying)
                    PlayConversationText(convEntity);
    }

    public void EnqueueConversationText(string text = "", float wordTerm = 0.1f, float endDelay = 1f, int fontSize = 36, AudioClip audioClip = null)
    {
        ConversationEntity convEntity = new ConversationEntity();

        convEntity.text = text;
        convEntity.wordTerm = wordTerm;
        convEntity.endDelay = endDelay;
        convEntity.fontSize = fontSize;
        convEntity.audioClip = audioClip;

        conversationQueue.Enqueue(convEntity);
    }

    void PlayConversationText(ConversationEntity convEntity)
    {
        StartCoroutine(IEPlayConversationText(convEntity));
    }

    IEnumerator IEPlayConversationText(ConversationEntity convEntity)
    {
        convEntity.isEnd = false;
        convEntity.isPlaying = true;

        conversationText.text = "";
        conversationText.fontSize = convEntity.fontSize;

        if (convEntity.audioClip)
            CKB_AudioManager.Instance.PlayOneShot(convEntity.audioClip);

        foreach (char ch in convEntity.text)
        {
            if (ch != ' ')
                yield return new WaitForSeconds(convEntity.wordTerm);

            conversationText.text += ch;
        }

        yield return new WaitForSeconds(convEntity.endDelay);

        convEntity.isEnd = true;
        convEntity.isPlaying = false;
    }

    public void AppearTextDialogue(float destY = 150f, float duration = 1.5f)
    {
        onStart();

        conversationText.text = "";

        isAppearingDialogue = true;
        
        dialoguePanel.DOAnchorPosY(destY, duration).SetEase(Ease.OutBounce).OnComplete(() => { isAppearingDialogue = false; });
    }

    public void DisappearTextDialogue(float destY = -300f, float duration = 1.5f)
    {
        StartCoroutine(IEDisappearTextDialogue(destY, duration));
    }

    IEnumerator IEDisappearTextDialogue(float destY, float duration)
    {
        while (conversationQueue.Count != 0)
            yield return null;

        dialoguePanel.DOAnchorPosY(destY, duration).SetEase(Ease.InBounce).OnComplete(onComplete);
    }
}
