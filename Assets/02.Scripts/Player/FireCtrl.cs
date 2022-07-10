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

    public float reloadTime; //����ӵ�                       

    public int reloadBulletcount; //���� ������ ����
    public int currentBulletCount; //���� źâ�� ���� �ִ� �Ѿ��� ����
    public int maxBulletCount; //�ִ� ���� ���� �Ѿ� ����
    public int carrybulletcount; // ���� �����ϰ� �ִ� �Ѿ� ����


    public Transform cam;
    Vector3 rot;

    public Animator anim;

    //�ѱ�ݵ�
    public float minX, maxX;
    public float minY, maxY;

    public ParticleSystem muzzleFlash;

    public static bool isReload = false; //�������ϴµ��ȿ��� �߻� ����
    public static bool isrunning = false; //�ٸ鼭 �߻� �ȵ� 
    public static bool iswalking = false;//�ȴ���

    public static bool isaiming = false; //���Ӵ��� ����


    // Muzzle flash�� Mesh Renderer ������Ʈ ĳ��

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
        // �������� �ν��Ͻ�ȭ�Ͽ� ����
        

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
