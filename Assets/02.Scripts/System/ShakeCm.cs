using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCm : MonoBehaviour
{

    private float shakeTime;
    private float shakeIntensity;

    private void Start()
    {
        OnShakeCam(0.1f, 1f);
    }

    /// <summary>
    /// </summary>
    /// <param name="shakeTime"></param>
    /// <param name="shakeIntensity"></param>

    public void OnShakeCam(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine(ShakeByPos());
        StartCoroutine(ShakeByPos());
    }


    private IEnumerator ShakeByPos()
    {
        Vector3 startPosition = transform.eulerAngles;

        float power = 5f;

        while(true)
        {
            float x = Random.Range(-1f, 1f);
            float y = 0;
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startPosition + new Vector3(x, y, z) * shakeIntensity * power);
            yield return new WaitForSeconds(0.05f);
            transform.rotation = Quaternion.Euler(startPosition);
        }

        
    }
}
