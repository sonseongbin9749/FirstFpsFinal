using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimeLine : MonoBehaviour
{

    public GameObject canvas;

    private void Update()
    {
        if(transform.localScale == Vector3.zero)
        {
            SceneManager.LoadScene("InGameIntro");
        }

        
    }

    public void Next()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void SetActiveTrue()
    {
        canvas.SetActive(true);
    }


}
