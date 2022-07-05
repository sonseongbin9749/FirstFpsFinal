using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject TargetObj;

    private Transform targetTransform;
    private Transform cameraTransform;

    public enum CameraTypeState { First, Second, Third }
    public CameraTypeState cameraType = CameraTypeState.First;

    public Transform firstCamTrns;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();

        if(TargetObj != null)
        {
            targetTransform = TargetObj.transform; 
        }
    }

    void LateUpdate()
    {
        
        if (TargetObj == null)
        {
            return;
        }
        if (targetTransform == null)
        {
           targetTransform = TargetObj.transform;
        }
        switch(cameraType)
        {
            case CameraTypeState.First:
                
                FirstCamera();
                break;
            case CameraTypeState.Second:
                SecondCamera();
                break;
            case CameraTypeState.Third:
                ThirdCamera();
                break;
        }
    }

    private void ThirdCamera()
    {
        throw new NotImplementedException();
    }

    private void SecondCamera()
    {
        throw new NotImplementedException();
    }

    private void FirstCamera()
    {
        cameraTransform.localPosition = firstCamTrns.position;
        cameraTransform.localRotation = firstCamTrns.rotation;
    }

   
}
