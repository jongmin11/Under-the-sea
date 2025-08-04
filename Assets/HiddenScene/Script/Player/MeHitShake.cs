using System.Collections;
using UnityEngine;

public class MeHitShake : MonoBehaviour
{
    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    public void Play(float duration = 0.3f, float magnitude = 0.2f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeRoutine = null;
    }
}
