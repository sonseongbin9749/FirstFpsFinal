using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float moveSpeed = 2.0f;
    //ĳ���� ȸ�� �̵� �ӵ�
    public float rotateMoveSpd = 100.0f;
    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 2.0f;
    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.1f;
    //���� ĳ���� �̵� ���� ��
    private Vector3 vecNowVelocity = Vector3.zero;
    //���� ĳ���� �̵� ���� ����
    private Vector3 vecMoveDirection = Vector3.zero;
    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;
    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    [SerializeField] float clampAngle = 70f; // ī�޶� ������ �ٲ� ���� ����

    private float rotx;//���콺 input�� ���� ������
    private float roty;

    public float sensitivity;

    private Animation playerAnim;

    // �ʱ� ���� ��
    private readonly float initHp = 100.0f;
    // ���� ���� ��
    public float currHp;

    // Hpbar �̹��� ����
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
        

        // hp �ʱ�ȭ
        currHp = initHp;
        // hpbar �̹��� ����
        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        // �ʱ�ȭ �� HP ǥ��
        DisplayHP();

        
    }

    

    

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        


        PlayerAnimation(h, v);
        Transform CameraTransform = Camera.main.transform;
        //���� ī�޶� �ٶ󺸴� ������ ����� � �����ΰ�.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        Vector3 up = new Vector3(forward.y, 0.0f , 0.0f);


        //�ɸ��Ͱ� �̵��ϰ��� �ϴ� ����
        Vector3 targetDirection = h * right + v * forward + up;


        // ������ �̵� ��
        Vector3 moveAmount = (vecMoveDirection * moveSpeed * Time.deltaTime);
        // ĳ���Ͱ� �ٴڿ� ��Ҵ��� Ȯ��
        if (IsGrounded())
            moveAmount.y = 0f;
        else
            moveAmount += Physics.gravity;
        // ĳ���� �̵�
        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);
        
    }

    bool IsGrounded()
    {
        // �Ʒ��ʰ� �浹�� ������ ��Ʈ�������� Ȯ��
        if ((collisionFlagsCharacter & CollisionFlags.Below) != 0)
            return true;
        else
            return false;
    }

    void vecDirectionChangeBody()
    {
        
        
            //�� ���� �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;
           
        
    }

    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ�
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {
            //��ȯ �ӵ� ���� ���� /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;
            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);
        }
        //�Ÿ� ũ��
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

            // HP ǥ��
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

        // ���� ����
        GameMgr.GetInstance().IsGameOver = true;
    }

    void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}
