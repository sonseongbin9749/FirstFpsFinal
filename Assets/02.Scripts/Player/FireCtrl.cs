using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bulletPrefab;

    // �Ѿ� �߻� ��ǥ
    public Transform firePos;

    // �ѼҸ� ����� Ŭ��
    public AudioClip fireSfx;
    private new AudioSource audio;

    private float currenttime = 0;

    public float accuracy; //��Ȯ��
    public float reloadTime; //����ӵ�                       

    public int reloadBulletcount; //���� ������ ����
    public int currentBulletCount; //���� źâ�� ���� �ִ� �Ѿ��� ����
    public int maxBulletCount; //�ִ� ���� ���� �Ѿ� ����
    public int carrybulletcount; // ���� �����ϰ� �ִ� �Ѿ� ����



    public Animator anim;

    public ParticleSystem muzzleFlash;

    private bool isReload = false; //�������ϴµ��ȿ��� �߻� ����
    public static bool isFineSightMode = false; //������
    public static bool isrunning = false; //�ٸ鼭 �߻� �ȵ� 

    public Vector3 fineSightOriginPos;


    // Muzzle flash�� Mesh Renderer ������Ʈ ĳ��

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (isReload == false)
        {
            // ���콺 ���� ��ư Ŭ�� ���� ��, 
            if (Input.GetMouseButton(0) && isReload == false)
            {
                if (currentBulletCount > 0)
                {
                    if (isrunning == false)
                    {
                        FireCal();
                        Fire();
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.R) && isReload == false && currentBulletCount < reloadBulletcount)
            {
                
                StartCoroutine(Reload());
            }
            //if(Input.GetMouseButtonDown(1) && isReload == false)
            //{
            //    FineSight();
            //}
        }

        
    }


    IEnumerator Reload()
    {
        if (carrybulletcount > 0)
        {
            isReload = true;


            anim.SetTrigger("Reload");

            yield return new WaitForSeconds(reloadTime);

            carrybulletcount += currentBulletCount;
            currentBulletCount = 0;

            if (carrybulletcount >= reloadBulletcount)
            {
                currentBulletCount = reloadBulletcount;
                carrybulletcount -= reloadBulletcount;
            }
            else
            {
                currentBulletCount = carrybulletcount;
                carrybulletcount = 0;
            }
            isReload = false;
        }
    }



    void FireCal()
    {
        if(currenttime > 0)
        {
            currenttime -= Time.deltaTime;
        }
    }


    void Fire()
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ����
        

        if (currenttime <= 0)
        {
            currenttime = 0.13f;

            currentBulletCount--;

            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            audio.PlayOneShot(fireSfx, 1f);


            StopAllCoroutines();
            

            muzzleFlash.Play();


        }
        
    }

    

    
}
