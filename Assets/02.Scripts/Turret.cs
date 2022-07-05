using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] Transform GunBody;
    [SerializeField] float range = 0f;
    [SerializeField] LayerMask layermask = 0;
    [SerializeField] float spinSpeed = 0f;
    [SerializeField] float _fireRate = 0f;
    [SerializeField] GameObject gm;
    [SerializeField] Transform trm, trm1,checktrm;
    public bool Canfix = false;

    float CurrnetFireRate = 0;

    Transform finaltarget = null;

    IEnumerator SerchEnemy()
    {
        while (true)
        {
            Collider[] cols = Physics.OverlapSphere(checktrm.position, range, layermask);
            Transform shortestTarget = null;

            if (cols.Length > 0)
            {
                float shortestdistance = Mathf.Infinity;
                foreach (Collider colTarget in cols)
                { 
                    float distance = Vector3.SqrMagnitude(checktrm.position - colTarget.transform.position);
                    if (shortestdistance > distance)
                    {
                        shortestdistance = distance;
                        shortestTarget = colTarget.transform;
                    }
                }
            }

            finaltarget = shortestTarget;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        CurrnetFireRate = _fireRate;
        StartCoroutine(SerchEnemy());
    }

    void Update()
    {

        
        if(Canfix == true)
        {
            turret();
            GunBody.gameObject.SetActive(true);
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach(Transform child in allChildren)
            {
                child.gameObject.layer = LayerMask.NameToLayer("FixedTurret");
                child.gameObject.tag = "FixedTurret";
            }
        }
        
    }

    
    void turret()
    {
        if (finaltarget != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(finaltarget.position - transform.position).normalized;
            Vector3 teuler = Quaternion.RotateTowards(GunBody.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;

            GunBody.rotation = Quaternion.Euler(0, teuler.y, 0);

            Quaternion firerotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
           
                CurrnetFireRate -= Time.deltaTime;
                if(CurrnetFireRate <= 0)
                {
                    CurrnetFireRate = _fireRate;
                    Instantiate(gm, trm.position, trm.rotation);
                    Instantiate(gm, trm1.position, trm1.rotation);
                }
            

        }
    }
}
