using System.Collections;
using UnityEngine;

public class EnemyTextHitEffect : MonoBehaviour
{
    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    /// <summary>
    /// 텍스트 피격 시 흔들림 효과 실행
    /// </summary>
    /// <param name="duration">흔들리는 시간</param>
    /// <param name="magnitude">흔들림 강도</param>
    public void Play(float duration = 0.1f, float magnitude = 0.1f)
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
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            // 자식 오브젝트만 흔들림
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원래 위치로 되돌림
        transform.localPosition = originalPos;
        shakeRoutine = null;
    }
}
