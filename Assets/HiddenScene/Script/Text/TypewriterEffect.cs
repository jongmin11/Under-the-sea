using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    private Coroutine typingCoroutine;

    public void StartTyping(TMP_Text target, string text, float speed = 0.05f, System.Action onComplete = null)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypingRoutine(target, text, speed, onComplete));
    }

    public void SkipToEnd(TMP_Text target, string fullText)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        target.text = fullText;
    }

    public  IEnumerator TypingRoutine(TMP_Text target, string text, float speed, System.Action onComplete)
    {
        target.text = "";

        foreach (char c in text)
        {
            target.text += c;
            yield return new WaitForSecondsRealtime(speed);
        }

        typingCoroutine = null;
        onComplete?.Invoke();
    }
}