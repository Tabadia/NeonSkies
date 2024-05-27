using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = transform.position.z; // Ensure the camera stays at its original z position
        transform.position = smoothedPosition;
    }

    public void ScreenShake(float duration, float intensity)
    {
        StartCoroutine(ExecuteScreenShake(duration, intensity));
    }
    
    private IEnumerator ExecuteScreenShake(float duration, float intensity)
    {
        Vector3 originalPosition = transform.localPosition;

        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForEndOfFrame();

            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = new Vector3(x + transform.localPosition.x, y + transform.localPosition.y, originalPosition.z);

        }

        transform.localPosition = originalPosition;
    }
}