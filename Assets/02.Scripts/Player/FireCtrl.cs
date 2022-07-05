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

    public float retroActionforce; //�ݵ� ����
    public float retroActionFineSightForce; //������ �� �ݵ�����

    [SerializeField] private Vector3 originPos; //���� ������ ��

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    private bool isReload = false; //�������ϴµ��ȿ��� �߻� ����
    public static bool isFineSightMode = false; //������
    public static bool isrunning = false; //�ٸ鼭 �߻� �ȵ� 


    private Recoil recoil;


    // Muzzle flash�� Mesh Renderer ������Ʈ ĳ��

    private void Start()
    {
        recoil = GetComponent<Recoil>();
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

    //public void CancelFineSight()
    //{
    //    if(isFineSightMode)
    //    {
    //        FineSight();
    //    }
    //}

    //private void FineSight()
    //{
    //    isFineSightMode = !isFineSightMode;
    //    anim.SetBool("FineSightMode", isFineSightMode); 

    //   if(isFineSightMode)
    //   {
    //        StopAllCoroutines();
    //        StartCoroutine(FineSightActive());
    //   }
    //   else
    //   {
    //        StopAllCoroutines();
    //        StartCoroutine(FineSightDeActive());
    //   }

    //}

    //IEnumerator  FineSightActive()
    //{
    //    while(transform.localPosition != fineSightOriginPos)
    //    {
    //        transform.localPosition = Vector3.Lerp(transform.localPosition, fineSightOriginPos, 0.2f);
    //        yield return null;
    //    }
    //}

    //IEnumerator  FineSightDeActive()
    //{
    //    while (transform.localPosition != originPos)
    //    {
    //        transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.2f);
    //        yield return null;
    //    }
    //}

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

    //IEnumerator RetroActionCoroutine()
    //{
    //    Vector3 recoilBack = new Vector3(retroActionforce, originPos.y, originPos.z);
    //    Vector3 retroActoinRecoilBack = new Vector3(retroActionFineSightForce, fineSightOriginPos.y, fineSightOriginPos.z);

    //    if(!isFineSightMode)
    //    {
    //        transform.localPosition = originPos;

    //        while(transform.localPosition.x <= retroActionforce - 0.02f)
    //        {
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilBack, 0.4f);
    //            yield return null;
    //        }
    //        while(transform.localPosition != originPos)
    //        {
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f);
    //            yield return null;  
    //        }


    //    }
    //    else
    //    {
    //        transform.localPosition = fineSightOriginPos;

    //        while (transform.localPosition.x <= retroActionFineSightForce - 0.02f)
    //        {
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, retroActoinRecoilBack, 0.4f);
    //            yield return null;
    //        }
    //        while (transform.localPosition != fineSightOriginPos)
    //        {
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, fineSightOriginPos, 0.1f);
    //            yield return null;
    //        }
    //    }
    //}

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
            //if (isFineSightMode)
            //{
            //    StartCoroutine(FineSightActive());
            //}
            //StartCoroutine(RetroActionCoroutine());

            muzzleFlash.Play();


        }
        
    }

    

    
}
