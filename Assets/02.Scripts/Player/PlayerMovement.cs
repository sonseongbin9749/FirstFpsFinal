using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float lookSensativity;

    //[SerializeField] private GameObject Gm;


    public static float speed = 0f;
    public float gravity = -9.81f;

    public float JumpHeight = 3f;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    Vector3 velocity;

    // 초기 생명 값
    private readonly float initHp = 100.0f;
    // 현재 생명 값
    public float currHp;

    // Hpbar 이미지 연결
    private Image hpBar;

    

    void Start()
    {
        //Gm.SetActive(true);     
        // hp 초기화
        currHp = initHp;
        // hpbar 이미지 연결
        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        // 초기화 된 HP 표시
        DisplayHP();
    }

    void Update()
    {
        Move();
    }

    

    void Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (FireCtrl.isaiming == true)
                {
                    FireCtrl.isrunning = false;
                    speed = 1f;
                }
                else if (FireCtrl.isaiming == false)
                {
                    Running();
                    FireCtrl.isrunning = true;
                    FireCtrl.iswalking = false;
                }
                

            }
            else
            {
                if (FireCtrl.isaiming == true)
                {
                    FireCtrl.isrunning = false;
                    speed = 1f;
                }
                else if(FireCtrl.isaiming == false)
                {
                    FireCtrl.isrunning = false;
                    speed = 5f;
                }

                    
                
                
            }
            
            
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            speed = 0;
        }

        if (speed <= 0)
        {
            FireCtrl.iswalking = false;
        }
        else if(speed > 0 && FireCtrl.isrunning == false)
        {
            FireCtrl.iswalking = true;
        }

        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity * 2);
        }
        
        
        
        
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * 2 * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Running()
    {
        speed = 7;
    }

    void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PUNCH") && currHp >= 0.0f)
        {
            currHp -= 10.0f;
            Debug.Log($"Player HP = {currHp}");

            // HP 표시
            DisplayHP();

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }
    void PlayerDie()
    {
        Debug.Log("Player Die!");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        // 게임 종료
        GameMgr.GetInstance().IsGameOver = true;
        //Gm.SetActive(false);

    }

    
}
