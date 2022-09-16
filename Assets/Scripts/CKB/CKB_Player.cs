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
    [HideInInspector]
    public State state;

    public enum DieType
    {
        FlyAway = 1,
        Disassemble = 2
    }

    [Header("튕겨 나가는 죽음 시 튕기는 정도")]
    public float dieFlyAwayForce;
    [Header("튕겨 나가는 죽음 시 회전하는 정도")]
    public float dieFlyAwayTorque;

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
                Die(DieType.Disassemble);
                Debug.Log("[CKB_Player] 플레이어 즉시 죽음");
            }
        }
    }

    public void Die(DieType dieType)
    {
        state = State.Die;

        Camera.main.transform.SetParent(null);

        switch (dieType)
        {
            case DieType.FlyAway :
                rb.useGravity = true;
                rb.AddExplosionForce(dieFlyAwayForce, transform.position + transform.forward * 0.5f + transform.up * 0.8f, 5f, 1f, ForceMode.Impulse);
                rb.AddTorque(transform.up * dieFlyAwayTorque, ForceMode.Impulse);
                break;
            case DieType.Disassemble :
                foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
                    rb.isKinematic = false;
                break;
        }

        CKB_UI_GameOver.Instance.PlayHitEffect();
        CKB_UI_GameOver.Instance.ShowGameOverUI(true);
    }
}
