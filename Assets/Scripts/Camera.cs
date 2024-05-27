using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 screenShakeOffset = Vector3.zero;

    //SCreen shake
    private float shakeY = 0;
    private float shakeX = 0;

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = transform.position.z; // Ensure the camera stays at its original z position
        screenShakeOffset = new Vector3(shakeX, shakeY, 0f);
        transform.position = smoothedPosition + screenShakeOffset;
    }

    public void ScreenShake(float duration, float intensity)
    {
        StartCoroutine(ExecuteScreenShake(duration, intensity));
    }
    
    private IEnumerator ExecuteScreenShake(float duration, float intensity)
    {
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForEndOfFrame();

            shakeY = Random.Range(-1f, 1f) * intensity;
            shakeX = Random.Range(-1f, 1f) * intensity; 
        }

        shakeX = 0;
        shakeY = 0;
    }
}