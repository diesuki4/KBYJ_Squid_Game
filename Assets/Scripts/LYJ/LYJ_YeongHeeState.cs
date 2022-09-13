using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_YeongHeeState : MonoBehaviour
{
    #region randomTime
    private float currentTime;
    private float mugunghwaTime;
    private float rayTime;
    private float bloomTime = 3;
    #endregion

    #region UI
    public GameObject canvasMugunghwa;
    public GameObject canvasBloom;
    public GameObject crosshair;
    #endregion

    #region movingDetect
    private LYJ_PlayerMoveDetect _playerMoveDetect;
    private bool targetForAttack;
    #endregion
    
    /* 상태머신 */
    public enum State
    {
        Idle,
        Mugunghwa,
        Bloom,
        End,
    }

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerMoveDetect = CKB_Player.Instance.GetComponent<LYJ_PlayerMoveDetect>();


        // state = State.Idle;
        state = State.Mugunghwa;
        
        CreatingRandomValue();
        canvasMugunghwa.SetActive(false);
        canvasBloom.SetActive(false);
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("targetForAttack: " + targetForAttack);
        
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Mugunghwa:
                UpdateMugunghwa();
                break;
            case State.Bloom:
                UpdateBloom();
                break;
            case State.End:
                UpdateEnd();
                break;
        }
    }

    
    private void CreatingRandomValue()
    {
        /* 무궁화 꽃이 피었습니다 랜덤 시간 */
        // 1 2~7 초 사이에 랜덤 값을 뽑아서
        // 2 MugunghwaTime / bloomTime 변수에 넣어준다
        mugunghwaTime = UnityEngine.Random.Range(2, 7);
        rayTime = mugunghwaTime - 1;
        Debug.Log("mugunghwaTime" + mugunghwaTime);
        // 3 어딘가에서 초기화해준다
    }

    private void UpdateIdle()
    {
        /* 대기화면 */
        // 게임 설명 (UI)
        // This is Red Light Green Light!
        // 제한 시간 내에 선 안으로 들어가면 통화입니다
        Debug.Log("state = State.Idle");
    }

    private void UpdateMugunghwa()
    {
        // Debug.Log("state = State.Mugunghwa");
        
        /* UI 표시 */
        // 1 시간이 흐르고
        currentTime += Time.deltaTime;
        // 2 mugunghwaTime 이 될 때까지
        if (currentTime < mugunghwaTime)
        {
            // 3 관련 UI를 표시한다
            canvasMugunghwa.SetActive(true);
        }
        else
        {
            currentTime = 0;
            canvasMugunghwa.SetActive(false);
            targetForAttack = false;
            crosshair.SetActive(false);
            state = State.Bloom;
        }
        
        /* 공격 */
        // 1 만약 타깃이 되면
        if (targetForAttack == true)
        {
            // 2 타깃 위에 crosshair 표시하고
            crosshair.SetActive(true);
            crosshair.transform.position = new Vector3(_playerMoveDetect.lastPos.x, _playerMoveDetect.lastPos.y + 2.5f, _playerMoveDetect.lastPos.z);
            crosshair.transform.eulerAngles = _playerMoveDetect.lastRot;
        }
        // 3 끝나기 1초전에 쏜다 
        if (currentTime >= rayTime)
        {
            // 4 플레이어 산산조각
            print("Player dead");
        }
    }

    private void UpdateBloom()
    {
        // Debug.Log("state = State.Bloom");
        
        /* 피었습니다 */
        // 1 시간이 흐르고
        currentTime += Time.deltaTime;
        // 2 bloomTime 이 될 때까지
        if (currentTime < bloomTime)
        {
            // 3 관련 UI를 표시한다
            canvasBloom.SetActive(true);
        }
        else
        {
            currentTime = 0;
            canvasBloom.SetActive(false);
            state = State.Mugunghwa;
        }
        
        /* 움직임 감지 */
        if (_playerMoveDetect.isMoving)
        {
            targetForAttack = true;
        }
    }


    private void UpdateEnd()
    {
        Debug.Log("state = State.End");
        // 다음 씬 로드
    }
}
