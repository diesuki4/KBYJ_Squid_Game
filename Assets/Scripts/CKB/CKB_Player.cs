using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Player : MonoBehaviour
{
    public static CKB_Player Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public enum State
    {
        Alive = 1,
        Stop = 2,
        Die = 4
    }
    public State state;

    [Header("죽음 시 튕겨 나가는 정도")]
    public float dieExplosionForce;
    [Header("죽음 시 회전하는 정도")]
    public float dieTorqueForce;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        state = State.Alive;
    }

    void Update()
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Die();
                Debug.Log("[CKB_Player] 플레이어 즉시 죽음");
            }
        }
    }

    public void Die()
    {
        state = State.Die;

        Camera.main.transform.SetParent(null);

        rb.useGravity = true;
        rb.AddExplosionForce(dieExplosionForce, transform.position + transform.forward * 0.5f + transform.up * 0.8f, 5f, 1f, ForceMode.Impulse);
        rb.AddTorque(transform.up * dieTorqueForce, ForceMode.Impulse);

        CKB_UI_GameOver.Instance.PlayHitEffect();
        CKB_UI_GameOver.Instance.ShowGameOverUI(true);
    }
}
