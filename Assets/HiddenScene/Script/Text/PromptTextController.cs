using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class PromptTextController : MonoBehaviour
{
    [Header("타이핑 설정")]
    public float typingSpeed = 0.02f;

    [Header("UI 연결")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI promptText;
    public Transform choiceGroupTransform;
    public Button yesButton;
    public Button noButton;

    [Header("UI 페이드")]
    public Graphic[] uiGraphicsToFade;

    public TypewriterEffect typewriter;

    private PromptLine[] promptLines;
    private PromptLine[] supportLines;

    private void AutoCollectGraphics()
    {
        var list = new List<Graphic>();

        // 대사 텍스트, 배경 등 모든 UI 요소 자동 수집
        list.AddRange(dialoguePanel.GetComponentsInChildren<Graphic>(true));

        // 버튼 그룹 내부 버튼 + 텍스트 전부 자동 수집
        list.AddRange(choiceGroupTransform.GetComponentsInChildren<Graphic>(true));

        uiGraphicsToFade = list.ToArray();
    }
    private void Awake()
    {
        AutoCollectGraphics();
        LoadAllLines();
        SetUIAlpha(0f); // 처음엔 모두 안 보이게
    }

    private void LoadAllLines()
    {
        var promptJson = Resources.Load<TextAsset>("prompt_lines");
        if (promptJson != null)
            promptLines = JsonHelper.FromJson<PromptLine>(promptJson.text);

        var supportJson = Resources.Load<TextAsset>("you_are_not_alone");
        if (supportJson != null)
            supportLines = JsonHelper.FromJson<PromptLine>(supportJson.text);
    }

    public void StartDialogueAfterDeath(int deathCount)
    {
        Time.timeScale = 0f;

        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
        {
            fader.FadeOut(Color.white, 1.0f, () =>
            {
                if (deathCount == 1)
                {
                    dialoguePanel.SetActive(true);
                    choiceGroupTransform.gameObject.SetActive(true);
                }

                SetUIAlpha(0f);
                StartCoroutine(FadeInUIObjects(uiGraphicsToFade, 0.5f));

                // ✅ 무조건 prompt 출력도 같이 실행
                StartCoroutine(ShowPromptLine(deathCount));

                if (deathCount >= 2)
                    StartCoroutine(ShowSupportMessages());
            });
        }
    }

    IEnumerator ShowPromptLine(int deathCount)
    {
        int index = Mathf.Min(deathCount - 1, promptLines.Length - 1);
        PromptLine line = promptLines[index];

        promptText.text = "";
        promptText.fontSize = line.fontSize > 0 ? line.fontSize : 36f;
        promptText.color = new Color(1, 1, 1, line.alpha > 0 ? line.alpha : 1f);
        promptText.alignment = TextAlignmentOptions.Center;

        if (!string.IsNullOrEmpty(line.font))
        {
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>(line.font);
            if (fontAsset != null)
                promptText.font = fontAsset;
        }

        yield return typewriter.TypingRoutine(promptText, line.text, typingSpeed, null);

        // ✅ 기존 버튼 활성화 제거하고 슬라이드 방식으로 대체
        StartCoroutine(SlideInButtons());
    }

    IEnumerator ShowSupportMessages()
    {
        if (supportLines == null || supportLines.Length == 0)
            yield break;

        choiceGroupTransform.gameObject.SetActive(false);

        int count = Mathf.Min(4, supportLines.Length);

        for (int i = 0; i < count; i++)
        {
            PromptLine line = supportLines[i];

            var spawner = FindObjectOfType<SupportMessageSpawner>();
            if (spawner != null)
                spawner.SpawnMessage(line);

            yield return new WaitForSecondsRealtime(1.2f);
        }

        choiceGroupTransform.gameObject.SetActive(true);
        SetButtonsInteractable(true);
    }


    public void OnClickContinue()
    {
        SetButtonsInteractable(false);
        StartCoroutine(FadeOutUIObjects(uiGraphicsToFade, 0.5f));
        StartCoroutine(ReviveAfterFadeIn());
    }

    public void OnClickGiveUp()
    {
        Application.Quit();
    }

    IEnumerator FadeOutUIObjects(Graphic[] targets, float duration)
    {
        float timeElapsed = 0f;
        float[] startAlphas = new float[targets.Length];

        for (int i = 0; i < targets.Length; i++)
            startAlphas[i] = targets[i].color.a;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            for (int i = 0; i < targets.Length; i++)
            {
                Color c = targets[i].color;
                c.a = Mathf.Lerp(startAlphas[i], 0, t);
                targets[i].color = c;
            }

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        foreach (var g in targets)
        {
            Color c = g.color;
            c.a = 0;
            g.color = c;
        }
    }

    IEnumerator FadeInUIObjects(Graphic[] targets, float duration)
    {
        float timeElapsed = 0f;
        float[] startAlphas = new float[targets.Length];

        for (int i = 0; i < targets.Length; i++)
            startAlphas[i] = targets[i].color.a;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            for (int i = 0; i < targets.Length; i++)
            {
                Color c = targets[i].color;
                c.a = Mathf.Lerp(startAlphas[i], 1f, t);
                targets[i].color = c;
            }

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        foreach (var g in targets)
        {
            Color c = g.color;
            c.a = 1f;
            g.color = c;
        }
    }

    IEnumerator ReviveAfterFadeIn()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        Debug.Log("🙋 ReviveAfterFadeIn() 실행됨");

        var player = FindObjectOfType<Me>();
        if (player != null)
        {
            Debug.Log("🙋 플레이어 Revive() 호출");
            player.Revive();  // ← 이게 반드시 isDead = false로 복구해줌
        }

        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
        {
            fader.FadeIn(Color.black, 1.0f);  // 화면 어두워졌다가 복귀
        }

        // 게임 시간 복구
        Time.timeScale = 1f;

        // UI는 알파값으로 전부 숨김 처리
        SetUIAlpha(0f);

    }

    private void SetButtonsInteractable(bool state)
    {
        yesButton.interactable = state;
        noButton.interactable = state;
    }

    private void SetUIAlpha(float alpha)
    {
        foreach (var g in uiGraphicsToFade)
        {
            Color c = g.color;
            c.a = alpha;
            g.color = c;
        }
    }

    IEnumerator SlideInButtons()
    {
        RectTransform rt = choiceGroupTransform.GetComponent<RectTransform>();

        Vector2 startPos = new Vector2(800f, rt.anchoredPosition.y);  // 화면 오른쪽 바깥
        Vector2 targetPos = new Vector2(0f, rt.anchoredPosition.y);   // 중앙 위치

        float duration = 0.5f;
        float elapsed = 0f;

        rt.anchoredPosition = startPos;
        choiceGroupTransform.gameObject.SetActive(true);
        SetButtonsInteractable(false);

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        SetButtonsInteractable(true);
    }

    void Start()
    {
        // 선택 버튼 처음에 오른쪽 화면 바깥에 위치시키기
        RectTransform rt = choiceGroupTransform.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(800f, rt.anchoredPosition.y); // ✅ 슬라이드 전 대기 위치
    }
}