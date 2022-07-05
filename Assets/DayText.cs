using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayText : MonoBehaviour
{
    Text text;
    public static int day = 0;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Day : " + day;
    }

    public void OnIndex()
    {
        day++;
    }
}
