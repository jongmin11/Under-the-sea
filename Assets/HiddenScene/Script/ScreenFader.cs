using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float defaultFadeDuration = 1f;
    public Color defaultFadeColor = Color.black;

    private Coroutine currentRoutine;

    public void FadeOut(Color? color = null, float? duration = null)
    {
        Color c = color ?? defaultFadeColor;
        float time = duration ?? defaultFadeDuration;

        fadeImage.gameObject.SetActive(true);

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeRoutine(1f, 0f, c, time));
    }

    public void FadeIn(Color? color = null, float? duration = null)
    {
        Color c = color ?? defaultFadeColor;
        float time = duration ?? defaultFadeDuration;

        fadeImage.gameObject.SetActive(true);

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeRoutine(0f, 1f, c, time));
    }

    private IEnumerator FadeRoutine(float fromAlpha, float toAlpha, Color baseColor, float duration)
    {
        float t = 0f;
        Color c = baseColor;
        c.a = fromAlpha;
        fadeImage.color = c;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            c.a = Mathf.Lerp(fromAlpha, toAlpha, lerp);
            fadeImage.color = c;
            yield return null;
        }

        c.a = toAlpha;
        fadeImage.color = c;


    }
}