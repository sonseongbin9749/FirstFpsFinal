using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    //캐릭터 직선 이동 속도 (걷기)
    public float moveSpeed = 2.0f;
    //캐릭터 회전 이동 속도
    public float rotateMoveSpd = 100.0f;
    //캐릭터 회전 방향으로 몸을 돌리는 속도
    public float rotateBodySpd = 2.0f;
    //캐릭터 이동 속도 증가 값
    public float moveChageSpd = 0.1f;
    //현재 캐릭터 이동 백터 값
    private Vector3 vecNowVelocity = Vector3.zero;
    //현재 캐릭터 이동 방향 벡터
    private Vector3 vecMoveDirection = Vector3.zero;
    //CharacterController 캐싱 준비
    private CharacterController controllerCharacter = null;
    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    [SerializeField] float clampAngle = 70f; // 카메라 각도가 바뀔때 각도 제한

    private float rotx;//마우스 input을 받을 변수들
    private float roty;

    public float sensitivity;

    private Animation playerAnim;

    // 초기 생명 값
    private readonly float initHp = 100.0f;
    // 현재 생명 값
    public float currHp;

    // Hpbar 이미지 연결
    private Image hpBar;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerAnim = GetComponent<Animation>();
        controllerCharacter = GetComponent<CharacterController>();
   

        

        playerAnim.Play("Idle");

        
        yield return new WaitForSeconds(0.3f);
        

        // hp 초기화
        currHp = initHp;
        // hpbar 이미지 연결
        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        // 초기화 된 HP 표시
        DisplayHP();

        
    }

    

    

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        


        PlayerAnimation(h, v);
        Transform CameraTransform = Camera.main.transform;
        //메인 카메라가 바라보는 방향이 월드상에 어떤 방향인가.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        Vector3 up = new Vector3(forward.y, 0.0f , 0.0f);


        //케릭터가 이동하고자 하는 방향
        Vector3 targetDirection = h * right + v * forward + up;


        // 프레임 이동 양
        Vector3 moveAmount = (vecMoveDirection * moveSpeed * Time.deltaTime);
        // 캐릭터가 바닥에 닿았는지 확인
        if (IsGrounded())
            moveAmount.y = 0f;
        else
            moveAmount += Physics.gravity;
        // 캐릭터 이동
        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);
        
    }

    bool IsGrounded()
    {
        // 아래쪽과 충돌되 었는지 비트연산으로 확인
        if ((collisionFlagsCharacter & CollisionFlags.Below) != 0)
            return true;
        else
            return false;
    }

    void vecDirectionChangeBody()
    {
        
        
            //내 몸통 바라봐야 하는 곳은 어디?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;
           
        
    }

    float getNowVelocityVal()
    {
        //현재 캐릭터가 멈춰 있다면
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //반환 속도 값은 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {
            //반환 속도 값은 현재 /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;
            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);
        }
        //거리 크기
        return vecNowVelocity.magnitude;
    }

    void PlayerAnimation(float h, float v)
    {
        if (h <= -0.1f)
            playerAnim.CrossFade("RunL", 0.25f);
        else if (h >= 0.1f)
            playerAnim.CrossFade("RunR", 0.25f);
        else if (v <= -0.1f)
            playerAnim.CrossFade("RunB", 0.25f);
        else if (v >= 0.1f)
            playerAnim.CrossFade("RunF", 0.25f);
        else
            playerAnim.CrossFade("Idle", 0.25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("PUNCH") && currHp >= 0.0f)
        {
            currHp -= 10.0f;
            Debug.Log($"Player HP = {currHp}");

            // HP 표시
            DisplayHP();

            if ( currHp <= 0.0f )
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach(GameObject monster in monsters )
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        // 게임 종료
        GameMgr.GetInstance().IsGameOver = true;
    }

    void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}
