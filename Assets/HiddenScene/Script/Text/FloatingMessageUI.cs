using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingMessageUI : MonoBehaviour
{
    public float floatAmplitude = 10f;
    public float floatSpeed = 1f;
    public float shakeAmount = 5f;
    public float shakeSpeed = 2f;
    public float typingSpeed = 0.05f;

    private RectTransform rect;
    private TextMeshProUGUI tmp;
    private string fullText = "";
    private float startTime;
    private Vector2 originalPos;
    private float originalAlpha = 1f;

    private Image fadeImage;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
            canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;

        if (GetComponent<GraphicRaycaster>() == null)
            gameObject.AddComponent<GraphicRaycaster>();

        GameObject fadeObj = GameObject.Find("FadeImage");
        if (fadeObj != null)
            fadeImage = fadeObj.GetComponent<Image>();

        originalPos = rect.anchoredPosition;
    }

    public void Setup(string message, TMP_FontAsset font, float fontSize, float alpha)
    {
        if (tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
            if (tmp == null)
            {
                Debug.LogError("TMP 없음!! 프리팹 확인 요망!");
                return;
            }
        }

        fullText = message;
        tmp.text = "";

        if (font != null) tmp.font = font;

        tmp.fontSize = fontSize > 0 ? fontSize : 48f;

        Color c = tmp.color;
        c.a = alpha > 0 ? alpha : 1f;
        tmp.color = c;

        originalAlpha = c.a;

        rect.anchoredPosition = Vector2.zero;
        originalPos = rect.anchoredPosition;

        startTime = Time.unscaledTime;

        StartCoroutine(TypeText());

    }

    System.Collections.IEnumerator TypeText()
    {
        tmp.text = "";
        foreach (char c in fullText)
        {
            tmp.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    void Update()
    {
        float t = Time.unscaledTime - startTime;
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
            if (rect == null) return;  // 그래도 없으면 포기
        }
        float floatOffset = Mathf.Sin(t * floatSpeed) * floatAmplitude;
        float shakeOffset = Mathf.Sin(t * shakeSpeed) * shakeAmount;
        rect.anchoredPosition = originalPos + new Vector2(shakeOffset, floatOffset);

        // ✅ 알파 완전 동기화 (fadeImage와 1:1 동일)
        float syncedAlpha = (fadeImage != null) ? fadeImage.color.a : originalAlpha;

        Color c = tmp.color;
        c.a = syncedAlpha;
        tmp.color = c;
        tmp.alpha = syncedAlpha;
        tmp.faceColor = new Color32(255, 255, 255, (byte)(syncedAlpha * 255));
    }

    public void Initialize(PromptLine line)
    {

        tmp.alignment = TextAlignmentOptions.Center;

        if (!string.IsNullOrEmpty(line.font))
        {
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>(line.font);
            if (fontAsset != null)
                tmp.font = fontAsset;
        }

        // 중앙 정렬
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
    }

}