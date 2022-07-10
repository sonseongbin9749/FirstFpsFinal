using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    public UnityEvent onInputSpace;

    [SerializeField] private float secondPerRealTimeSecond;

    private bool isNight = false;

    [SerializeField] private float time;

    [SerializeField] private Text win;
    [SerializeField] private float nightFogDensity;
    private float dayFogDensity;
    [SerializeField] private float fogDensityCalc;
    private float currentFogDensity;
    public static bool day = false;
    public bool isday = false;

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) isNight = true;
        else if (transform.eulerAngles.x >= -110) isNight = false;

        if (isNight)
        {
            isday = false;
            GameMgr.createTime = 0.8f;
            
            
            if (currentFogDensity <= nightFogDensity && currentFogDensity <= 0.01f)
            {
                currentFogDensity += 0.001f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            
        }

        else
        {
            
            GameMgr.createTime = 3.0f;

            if(isday == false)
            {
                isday = true;
                onInputSpace.Invoke();
            }
            
            if (currentFogDensity >= dayFogDensity && currentFogDensity >= 0)
            {
                currentFogDensity -= 0.001f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }

        if(DayText.day == 7)
        {
            win.gameObject.SetActive(true);
            time += Time.deltaTime;
            if(time > 7)
            {
                DayText.day = 0;
                SceneManager.LoadScene("StartMenu");
            }
            
        }

        



    }
}
