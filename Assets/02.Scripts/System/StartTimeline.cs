using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartTimeline : MonoBehaviour
{

    public PlayableDirector a;
    void Start()
    {
        a.Play();
    }



    
}
