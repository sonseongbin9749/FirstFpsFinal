using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCtrl : MonoBehaviour
{
    [SerializeField] private float range;

    public bool pickupActive,_canfix = false;

    private RaycastHit hitinfosave;

    public GameObject itemfan;

    private Turret turret;

    [SerializeField] private LayerMask layerMask;


    [SerializeField]
    private Text actionText, turretactionText, fixedturrettext;

   

    void Update()
    {
        Checkitem();
        TryAction();
        
    }


    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            
            Checkitem();
            CanPickup();
            CanturretFix();
            
        }
        
    }

    private void CanturretFix()
    {
        if(ItemText.scoreValue >= 3 && _canfix == true)
        {
            turret = hitinfosave.transform.GetComponentInParent<Turret>();
            Debug.Log(hitinfosave.transform.gameObject.name);
            turret.Canfix = true;
            UnknownItem item = itemfan.GetComponent<UnknownItem>();
            item.UnknownItemsystem(-3);
        }
    }

    private void CanPickup()
    {
        if(pickupActive)    
        {
            if(hitinfosave.transform != null)
            {
                UnknownItem item = itemfan.GetComponent<UnknownItem>();
                item.UnknownItemsystem(1);


                Destroy(hitinfosave.transform.gameObject); 
                InfoDisappear();
            }
            
        }

    }

    private void Checkitem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfosave, range, layerMask))
        {
            if (hitinfosave.transform.tag == "Item")
            {
                _canfix = false;
                ItemInfoAppear();
            }
            else if (hitinfosave.transform.tag == "Turret")
            {
                _canfix = true;
                
                TurretInfoAppear();

                
            }
            else if(hitinfosave.transform.tag == "FixedTurret")
            {
                turretactionText.gameObject.SetActive(false);
                _canfix = false;
                FixedTurretInfoAppear();
                
            }
        }
        else
        {
            InfoDisappear();

        }
    }

    private void TurretInfoAppear()
    {
        
        turretactionText.gameObject.SetActive(true);
        turretactionText.text = "¸Á°¡Áø ÅÍ·¿";
    }
    private void FixedTurretInfoAppear()
    {

       fixedturrettext.gameObject.SetActive(true);
       fixedturrettext.text = "ÅÍ·¿";
    }

    private void InfoDisappear()
    {
        _canfix = false;
        pickupActive = false;
        actionText.gameObject.SetActive(false);
        turretactionText.gameObject.SetActive(false);
        fixedturrettext.gameObject.SetActive(false);

    }

    private void ItemInfoAppear()
    {
        
        pickupActive = true;
        actionText.gameObject.SetActive(true); 
        actionText.text = "¾Ë¼ö¾ø´Â ºÎÇ°Á¦·á" + "<color=yellow>" + "(F)" + "</color>";
    }
}
