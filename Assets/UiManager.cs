using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private FireCtrl firectrl;


    [SerializeField] private Text[] text_bullet;

    private void Start()
    {
    }

    void Update()
    {
        Checkbullet(); 
    }



    private void Checkbullet()
    {
        text_bullet[0].text = firectrl.carrybulletcount.ToString();
        text_bullet[1].text = firectrl.reloadBulletcount.ToString();
        text_bullet[2].text = firectrl.currentBulletCount.ToString();
    }
}
