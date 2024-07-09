using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.5f;
    public float magnitude = 0.1f;
    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        StartShake(10f, 0.2f);
    }


    public void StartShake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            this.duration = duration;
            this.magnitude = magnitude;
            StartCoroutine(Shake());
        }
    }

    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = originalPosition;
        isShaking = false;
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
