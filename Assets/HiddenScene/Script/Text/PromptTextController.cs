using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptTextController : MonoBehaviour
{
    [Header("텍스트 소스")]
    public string promptJsonFile = "prompt_lines";
    public string supportJsonFile = "you_are_not_alone";

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

    [Header("메시지 데이터")]
    public PromptLine[] promptLines;
    public PromptLine[] supportLines;

    private PromptLine[] lines;
    private int currentIndex = 0;
    private bool dialogueStarted = false;

    public TypewriterEffect typewriter;

    private void Awake()
    {
        LoadAllLines();
    }

    void LoadAllLines()
    {
        var promptJson = Resources.Load<TextAsset>(promptJsonFile);
        if (promptJson != null)
            promptLines = JsonHelper.FromJson<PromptLine>(promptJson.text);
        else
            promptLines = new PromptLine[0];

        var supportJson = Resources.Load<TextAsset>(supportJsonFile);
        if (supportJson != null)
            supportLines = JsonHelper.FromJson<PromptLine>(supportJson.text);
        else
            supportLines = new PromptLine[0];
    }

    public void StartDialogueAfterDeath(int deathCount)
    {
        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
        {
            fader.FadeOut(Color.white, 1.0f, () =>
            {
                lines = promptLines;
                currentIndex = 0;
                dialoguePanel.SetActive(true);
                StartCoroutine(ShowDialogueSequence());

                if (deathCount >= 2)
                {
                    StartSupportMessageSequence();
                }
            });
        }
    }

    IEnumerator ShowDialogueSequence()
    {
        if (lines.Length == 0) yield break;

        PromptLine line = lines[currentIndex];

        promptText.text = "";
        promptText.fontSize = line.fontSize > 0 ? line.fontSize : 36f;
        promptText.color = new Color(1, 1, 1, line.alpha > 0 ? line.alpha : 1f);

        if (!string.IsNullOrEmpty(line.font))
        {
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>(line.font);
            if (fontAsset != null)
                promptText.font = fontAsset;
        }

        yield return typewriter.TypingRoutine(promptText, line.text, typingSpeed, null);

        choiceGroupTransform.gameObject.SetActive(true);
        yesButton.interactable = true;
        noButton.interactable = true;
    }

    public void OnClickContinue()
    {
        yesButton.interactable = false;
        noButton.interactable = false;

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

    IEnumerator ReviveAfterFadeIn()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        var player = FindObjectOfType<Me>();
        if (player != null)
            player.Revive();

        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
            fader.FadeIn(Color.white, 1.0f);

        dialogueStarted = false;
        yesButton.interactable = true;
        noButton.interactable = true;
        choiceGroupTransform.gameObject.SetActive(false);
    }

    public void StartSupportMessageSequence()
    {
        StartCoroutine(ShowSupportMessages());
    }

    IEnumerator ShowSupportMessages()
    {
        Time.timeScale = 0f;
        int count = Mathf.Min(4, supportLines.Length);

        for (int i = 0; i < count; i++)
        {
            PromptLine line = supportLines[i];

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

            yield return typewriter.TypingRoutine(promptText, line.text, typingSpeed, null);

            var spawner = FindObjectOfType<SupportMessageSpawner>();
            if (spawner != null)
                spawner.SpawnMessage(line);

            yield return new WaitForSecondsRealtime(1.2f);
        }

        choiceGroupTransform.gameObject.SetActive(true);
        yesButton.interactable = true;
        noButton.interactable = true;
    }
}