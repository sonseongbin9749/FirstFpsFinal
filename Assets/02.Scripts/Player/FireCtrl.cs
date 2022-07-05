using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // 총알 프리팹
    public GameObject bulletPrefab;

    // 총알 발사 좌표
    public Transform firePos;

    // 총소리 오디오 클립
    public AudioClip fireSfx;
    private new AudioSource audio;

    private float currenttime = 0;

    public float accuracy; //정확도
    public float reloadTime; //연사속도                       

    public int reloadBulletcount; //총의 재장전 개수
    public int currentBulletCount; //현재 탄창에 남아 있는 총알의 개수
    public int maxBulletCount; //최대 소유 가능 총알 개수
    public int carrybulletcount; // 현재 소유하고 있는 총알 개수

    public float retroActionforce; //반동 세기
    public float retroActionFineSightForce; //정조준 시 반동세기

    [SerializeField] private Vector3 originPos; //본래 포지션 값

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    private bool isReload = false; //재장전하는동안에는 발사 금지
    public static bool isFineSightMode = false; //정조준
    public static bool isrunning = false; //뛰면서 발사 안됨 


    private Recoil recoil;


    // Muzzle flash의 Mesh Renderer 컴포넌트 캐싱

    private void Start()
    {
        recoil = GetComponent<Recoil>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (isReload == false)
        {
            // 마우스 왼쪽 버튼 클릭 했을 때, 
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
        // 프리팹을 인스턴스화하여 생성
        

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
