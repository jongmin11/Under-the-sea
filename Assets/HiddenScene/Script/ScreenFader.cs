using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float defaultFadeDuration = 1f;
    public Color defaultFadeColor = Color.black;

    private Coroutine currentRoutine;

    public void FadeOut(Color color, float duration, Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeRoutine(0f, 1f, color, duration, onComplete));
    }

    public void FadeIn(Color color, float duration)
    {
        fadeImage.gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeRoutine(1f, 0f, color, duration, null));
    }

    IEnumerator FadeRoutine(float fromAlpha, float toAlpha, Color color, float duration, Action onComplete)
    {
        float timeElapsed = 0f;
        fadeImage.color = new Color(color.r, color.g, color.b, fromAlpha);

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
        onComplete?.Invoke(); // 💥 콜백 실행
    }
}