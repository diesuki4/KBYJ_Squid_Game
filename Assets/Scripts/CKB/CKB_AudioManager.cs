using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_AudioManager : MonoBehaviour
{
    public static CKB_AudioManager Instance;

    AudioSource asrc;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        asrc = GetComponent<AudioSource>();
    }

    void Update() { }

    public void PlayOneShot(AudioClip audioClip)
    {
        asrc.PlayOneShot(audioClip);
    }
}
