using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRot = 0f;

    IEnumerator Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = 0f;
        yield return new WaitForSeconds(0.4f);
        mouseSensitivity = 100f;
    }

    private void Update()
    {
        Camerarot();   
    }
    

    void Camerarot()
    {

        float _xRotation = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float _yRotation = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= _yRotation;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        playerBody.Rotate(Vector3.up * _xRotation);
    }
}
