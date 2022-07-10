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

    public float reloadTime; //연사속도                       

    public int reloadBulletcount; //총의 재장전 개수
    public int currentBulletCount; //현재 탄창에 남아 있는 총알의 개수
    public int maxBulletCount; //최대 소유 가능 총알 개수
    public int carrybulletcount; // 현재 소유하고 있는 총알 개수


    public Transform cam;
    Vector3 rot;

    public Animator anim;

    //총기반동
    public float minX, maxX;
    public float minY, maxY;

    public ParticleSystem muzzleFlash;

    public static bool isReload = false; //재장전하는동안에는 발사 금지
    public static bool isrunning = false; //뛰면서 발사 안됨 
    public static bool iswalking = false;//걷는중

    public static bool isaiming = false; //에임당기고 해제


    // Muzzle flash의 Mesh Renderer 컴포넌트 캐싱

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {

        rot = cam.transform.localRotation.eulerAngles;
        if (rot.x != 0 || rot.y != 0)
        {
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3);
        }
        if (isReload == false)
        {
            if (Input.GetMouseButton(0))
            {
                if (currentBulletCount > 0)
                {
                    if (isrunning == false && isaiming == true || isrunning == false && iswalking == false && isaiming == false)
                    {
                        FireCal();
                        Fire();
                    }
                    

                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                
                ADS();
            }
            if (Input.GetKeyDown(KeyCode.R) && currentBulletCount < reloadBulletcount)
            {
                isaiming = false;
                anim.SetBool("Aim", false);
                StartCoroutine(Reload());
            }

            Ctrl();

        }
        
    }

    private void ADS()
    {
        if (isrunning == false)
        {
            isaiming = !isaiming;
            anim.SetBool("Aim", isaiming);
        }
    }

    private void Ctrl()
    {
        if (iswalking == false)
        {
            anim.SetBool("Walk", false);
        }
        if (iswalking == true)
        {
            anim.SetBool("Walk", true);
        }
        if (isrunning == false)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Idle", false);
        }
        if (isrunning == true)
        {
            anim.SetBool("Run", true);

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
        // 프리팹을 인스턴스화하여 생성
        

        if (currenttime <= 0)
        {
            currenttime = 0.13f;

            currentBulletCount--;

            recoil();
            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            audio.PlayOneShot(fireSfx, 1f);

            

            StopAllCoroutines();
            

            muzzleFlash.Play();


        }
        
    }

    private void recoil()
    {

        float recX = Random.Range(minX, maxX);
        float recY = Random.Range(minY, maxY);
        cam.transform.localRotation = Quaternion.Euler(rot.x - recY, rot.y + recX, rot.z);

    }

    

    

    
}
