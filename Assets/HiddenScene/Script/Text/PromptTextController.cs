using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PromptTextController : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject dialoguePanel;
    public TMP_Text promptText;
    public RectTransform choiceGroupTransform;
    public Button yesButton;
    public Button noButton;

    [Header("연출 설정")]
    public float waitAfterFade = 2.0f;
    public float typingSpeed = 0.06f;
    public float slideDistance = 300f;
    public float slideDuration = 0.4f;

    [Header("타이핑 컨트롤러")]
    public TypewriterEffect typewriter;

    [Header("알파값 줄일 UI들")]
    public List<Graphic> uiGraphicsToFade; // TMP_Text, Image 등

    private PromptLine[] lines;
    PromptLine[] despairLines;
    PromptLine[] supportLines;
    private int currentIndex = 0;
    private Vector2 originalButtonPosition;


    public string jsonFileName = "you_are_not_alone";
    void Start()
    {
        // 초기화
        dialoguePanel.SetActive(false);
        choiceGroupTransform.gameObject.SetActive(false);
        promptText.text = "";

        originalButtonPosition = choiceGroupTransform.anchoredPosition;
        
        LoadAllLines();

    }

    void LoadAllLines()
    {
        var promptJson = Resources.Load<TextAsset>("prompt_lines");
        TextAsset supportJson = Resources.Load<TextAsset>(jsonFileName);

        if (promptJson != null)
            despairLines = JsonHelper.FromJson<PromptLine>(promptJson.text);
        else
            despairLines = new PromptLine[0];

        if (supportJson != null)
            supportLines = JsonHelper.FromJson<PromptLine>(supportJson.text);
        else
        {
            Debug.LogError($"지원 메시지 JSON 파일이 존재하지 않음: {jsonFileName}");
            supportLines = new PromptLine[0];
        }

        Debug.Log($"📂 불러온 JSON: {jsonFileName}");
    }

    PromptLine GetNextLine()
    {
        if (lines == null || lines.Length == 0) return null;
        PromptLine line = lines[currentIndex++];
        if (currentIndex >= lines.Length)
            currentIndex = 0;
        return line;
    }

    IEnumerator ShowDialogueSequence()
    {
        yield return new WaitForSecondsRealtime(waitAfterFade);

        dialoguePanel.SetActive(true);

        // 알파 복원
        foreach (var g in uiGraphicsToFade)
        {
            if (g == null) continue;
            Color c = g.color;
            c.a = 1f;
            g.color = c;
            g.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(0.2f);

        PromptLine line = GetNextLine();
        if (line != null && typewriter != null)
        {
            promptText.fontSize = line.fontSize > 0 ? line.fontSize : 36f;

            Color c = promptText.color;
            c.a = line.alpha > 0f ? line.alpha : 1f;
            promptText.color = c;

            if (!string.IsNullOrEmpty(line.font))
            {
                var fontAsset = Resources.Load<TMP_FontAsset>(line.font);
                if (fontAsset != null)
                    promptText.font = fontAsset;
            }

            typewriter.StartTyping(promptText, line.text, typingSpeed, () =>
            {
                StartCoroutine(SlideInFromRight(choiceGroupTransform, originalButtonPosition, slideDistance, slideDuration));

                // ✅ 응원 메시지 여기서 띄운다 (타이핑 + 슬라이드 끝나는 타이밍)
                var spawner = FindObjectOfType<SupportMessageSpawner>();
                if (spawner != null)
                    spawner.SpawnMessage(line);
            });
        }
    }

    IEnumerator SlideInFromRight(RectTransform target, Vector2 finalPos, float distance = 300f, float duration = 0.4f)
    {
        target.gameObject.SetActive(true);

        Vector2 startPos = finalPos + new Vector2(distance, 0f);
        target.anchoredPosition = startPos;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            target.anchoredPosition = Vector2.Lerp(startPos, finalPos, lerp);
            yield return null;
        }

        target.anchoredPosition = finalPos;
    }

    public void OnClickContinue()
    {
        Debug.Log("🛑 '아니요(계속)' 선택됨");

        // 1. 버튼 비활성화
        yesButton.interactable = false;
        noButton.interactable = false;

        // 2. UI 요소 사라지게
        StartCoroutine(FadeOutUIObjects(uiGraphicsToFade, 0.5f));
 
        // 4. 1초 후 revive
        StartCoroutine(ReviveAfterFadeIn());


    }

    public void OnClickGiveUp()
    {
        Debug.Log("💥 네(포기) 선택됨 – 게임 종료");
        Application.Quit();
    }

    IEnumerator ReviveAfterFadeIn()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        var player = FindObjectOfType<Me>();
        if (player != null)
            player.Revive();

        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
            fader.FadeOut(Color.white, 1.0f);

        yesButton.interactable = true;
        noButton.interactable = true;
        choiceGroupTransform.gameObject.SetActive(false);
    }

    IEnumerator FadeOutUIObjects(List<Graphic> uiObjects, float duration = 0.5f)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            foreach (var g in uiObjects)
            {
                if (g == null) continue;
                Color c = g.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                g.color = c;
            }

            yield return null;
        }

        foreach (var g in uiObjects)
        {
            if (g == null) continue;
            Color c = g.color;
            c.a = 0f;
            g.color = c;
            g.gameObject.SetActive(false);
        }
    }

    public void StartSupportMessageSequence()
    {
        StartCoroutine(ShowSupportMessages());
    }

    IEnumerator ShowSupportMessages()
    {

        // 전체 UI 중지 및 패널만 켜기
        Time.timeScale = 0f;
        dialoguePanel.SetActive(true);
        choiceGroupTransform.gameObject.SetActive(false);
        promptText.text = "";

        // supportLines 배열 3~5개만 표시
        int count = Mathf.Min(4, supportLines.Length);
        for (int i = 0; i < count; i++)
        {
            PromptLine line = supportLines[i];

            // 텍스트 설정
            promptText.text = "";
            promptText.fontSize = line.fontSize > 0 ? line.fontSize : 48f;
            promptText.alignment = TextAlignmentOptions.Center;
            promptText.color = new Color(1, 1, 1, line.alpha > 0 ? line.alpha : 1f);

            if (!string.IsNullOrEmpty(line.font))
            {
                TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>(line.font);
                if (fontAsset != null)
                    promptText.font = fontAsset;
            }

            // 타이핑 효과
            yield return typewriter.TypingRoutine(promptText, line.text, typingSpeed, null);

            // 다음 문장 뜨기 전 잠깐 기다리기
            yield return new WaitForSecondsRealtime(1.2f);
        }

        // 마지막에 버튼 등장 (Revive 여부 선택)
        choiceGroupTransform.gameObject.SetActive(true);
        yesButton.interactable = true;
        noButton.interactable = true;
    }
    public void StartDialogueAfterDeath(int deathCount)
    {
        lines = supportLines;
        if (deathCount < 3)
        {
            lines = despairLines;
            currentIndex = Mathf.Min(deathCount - 1, despairLines.Length - 1);
        }
        else
        {
            lines = supportLines;
            currentIndex = Mathf.Min(deathCount - 3, supportLines.Length - 1);
        }

        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogueSequence());
    }
}