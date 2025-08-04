using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class FloatingMessageUI : MonoBehaviour
{
    public float floatAmplitude = 10f;
    public float floatSpeed = 1f;
    public float shakeAmount = 5f;
    public float shakeSpeed = 2f;
    public float typingSpeed = 0.05f;

    private RectTransform rect;
    private TextMeshProUGUI tmp;

    private string textTyping = "";     // 외국어 (타이핑)
    private string textTranslated = ""; // 한국어 (최종 번역)

    private float startTime;
    private Vector2 originalPos;
    private float originalAlpha = 1f;

    private Image fadeImage;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();

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

    public void Setup(PromptLine line, TMP_FontAsset font, float fontSize, float alpha)
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

        textTyping = line.foreignText;
        textTranslated = line.text;
        tmp.text = "";

        if (font != null) tmp.font = font;
        tmp.fontSize = fontSize > 0 ? fontSize : 48f;

        Color c = tmp.color;
        c.a = alpha > 0 ? alpha : 1f;
        tmp.color = c;

        originalAlpha = c.a;
        startTime = Time.unscaledTime;

        StartCoroutine(TypeForeignThenTranslateWithGlitch());
    }

    IEnumerator TypeForeignThenTranslateWithGlitch()
    {
        tmp.text = "";

        // ① 외국어 타이핑 출력
        foreach (char c in textTyping)
        {
            tmp.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        yield return new WaitForSecondsRealtime(0.3f);

        // ② 치지직 효과 출력
        float glitchDuration = 0.4f;
        float elapsed = 0f;

        while (elapsed < glitchDuration)
        {
            tmp.text = GenerateGlitchedText(textTyping.Length);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // ③ 최종 번역 문장으로 교체
        tmp.text = textTranslated;
    }

    string GenerateGlitchedText(int length)
    {
        const string glitchChars = "!@#$%^&*<>/?|1234567890-=+~";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < length; i++)
        {
            char c = glitchChars[Random.Range(0, glitchChars.Length)];
            sb.Append(c);
        }

        return sb.ToString();
    }

    void Update()
    {
        float t = Time.unscaledTime - startTime;
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
            if (rect == null) return;
        }

        float floatOffset = Mathf.Sin(t * floatSpeed) * floatAmplitude;
        float shakeOffset = Mathf.Sin(t * shakeSpeed) * shakeAmount;
        rect.anchoredPosition = originalPos + new Vector2(shakeOffset, floatOffset);

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

        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
    }

    public void SetOrigin(Vector2 pos)
    {
        originalPos = pos;
    }
}