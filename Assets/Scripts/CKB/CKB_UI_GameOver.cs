using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;

public class CKB_UI_GameOver : MonoBehaviourPunCallbacks
{
    public static CKB_UI_GameOver Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    CanvasGroup cnvsGrp;

    void Start()
    {
        cnvsGrp = GetComponent<CanvasGroup>();

        ShowGameOverUI(false);
    }

    public void ShowGameOverUI(bool show, float duration = 2f, float delay = 0.5f)
    {
        cnvsGrp.DOFade(1f, (show ? duration : 0)).SetDelay(show ? delay : 0).OnComplete(() => { ActiveCanvasGroup(show); });
    }

    void ActiveCanvasGroup(bool active)
    {
        cnvsGrp.alpha = Convert.ToInt32(active);
        cnvsGrp.interactable = active;
        cnvsGrp.blocksRaycasts = active;
    }

    void ActiveAllUI(bool active)
    {
        foreach (Transform tr in transform)
            tr.gameObject.SetActive(active);
    }

    public void PlayHitEffect(float duration = 0.2f)
    {
        StartCoroutine(IEPlayHitEffect(duration));
    }

    IEnumerator IEPlayHitEffect(float duration)
    {
        Image backgroundImage = transform.Find("Background Image").GetComponent<Image>();

        ActiveAllUI(false);

        Color o_color = backgroundImage.color;
        
        backgroundImage.color = new Color(255, 0, 0, 50) / 255;
        
        backgroundImage.gameObject.SetActive(true);
        
        ActiveCanvasGroup(true);
        
        yield return new WaitForSeconds(duration);
        
        ActiveCanvasGroup(false);
        
        backgroundImage.color = o_color;
        
        ActiveAllUI(true);
    }

    public void OnRespawnButtonClick()
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            Debug.Log("[CKB_UI_GameOver] 부활 버튼 클릭");
            
            print("SceneManager.GetActiveScene().name" + SceneManager.GetActiveScene().name);
            print("SceneManager.GetActiveScene().buildIndex: " + SceneManager.GetActiveScene().buildIndex);

            if (CKB_GameManager.Instance.photonMode)
                PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnReturnLobbyButtonClick()
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            Debug.Log("[CKB_UI_GameOver] 로비로 돌아가기 버튼 클릭");

            if (CKB_GameManager.Instance.photonMode)
                PhotonNetwork.LoadLevel("WaitingroomScene");
        }
    }

    public void OnObserveButtonClick()
    {
        ActiveCanvasGroup(false);

        Camera.main.GetComponent<CKB_CameraObserve>().enabled = true;
    }
}
