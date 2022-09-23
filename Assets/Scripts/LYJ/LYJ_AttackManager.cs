using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LYJ_AttackManager : MonoBehaviour
{
    public bool isChangeWeapon;
    public GameObject baseball;

    public GameObject pushUI;
    public GameObject baseballUI;

    [SerializeField] [Range(0.0f, 10.0f)] private float pushTime; 

    private Animator anim;
    
    public enum AttackState
    {
        Idle = 1,
        Push = 2,
        Baseball = 4,
    }
    
    // [HideInInspector]
    public AttackState attackState;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                UpdateIdle();
                break;
            case AttackState.Push:
                UpdatePush();
                break;
            case AttackState.Baseball:
                UpdateBaseball();
                break;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            attackState = AttackState.Push;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            attackState = AttackState.Baseball;
        }
        if (Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Alpha9))
        {
            attackState = AttackState.Idle;
        }
    }

    private void UpdateIdle()
    {
        attackState = AttackState.Idle;
        pushUI.GetComponent<Image>().enabled = false;
        baseballUI.GetComponent<Image>().enabled = false;
        
        anim.SetTrigger("Idle");
    }

    private void UpdatePush()
    {
        attackState = AttackState.Push;
        Debug.Log("attackState: " + attackState);
        pushUI.GetComponent<Image>().enabled = true;
        baseball.SetActive(false);
        anim.SetTrigger("Push");
        
        // 밀치는 코드 Lerp
    }

    private void UpdateBaseball()
    {
        attackState = AttackState.Baseball;
        Debug.Log("attackState: " + attackState);
        baseballUI.GetComponent<Image>().enabled = true;
        
        //애니메이터
        baseball.SetActive(true);
        // StartCoroutine("IENoBaseball");
    }

    IEnumerator IENoBaseball()
    {
        anim.SetTrigger("Baseball");
        yield return new WaitForSeconds(1);
    }
}
