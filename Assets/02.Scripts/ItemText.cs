using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    Text text;
    public static int scoreValue = 3;

    private void Start()
    {
        text = GetComponent<Text>();

    }

    private void Update()
    {
        text.text = "�˼� ���� ��ǰ ��� : " + scoreValue;
    }

    
}
