using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CKB_UI_Menu : MonoBehaviour
{
    public static CKB_UI_Menu Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void ActiveAllUI(bool active)
    {
        foreach (Transform tr in transform)
            tr.gameObject.SetActive(active);
    }

    public void OnMenuButtonClick()
    {
        GameObject openMenuButton =  transform.Find("Open Menu Button").gameObject;

        if (openMenuButton.activeSelf)
        {
            ActiveAllUI(true);

            openMenuButton.SetActive(false);
        }
        else
        {
            ActiveAllUI(false);

            openMenuButton.SetActive(true);
        }
    }

    public void OnRespawnButtonClick()
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            Debug.Log("[CKB_UI_Menu] 부활 버튼 클릭");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnReturnLobbyButtonClick(string lobbySceneName)
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            Debug.Log("[CKB_UI_Menu] 로비로 돌아가기 버튼 클릭");

            if (lobbySceneName != "")
                SceneManager.LoadScene(lobbySceneName);
        }
    }
}
