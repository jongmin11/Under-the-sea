using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 사망 시 암전과 게임오버 UI 전체를 자연스럽게 출력하는 컨트롤러
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("페이드 이미지")]
    [SerializeField] private Image fadeImage;

    [Header("페이드 설정")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("점수 UI")]
    [SerializeField] private TMP_Text DiecurrentScoreText;
    [SerializeField] private TMP_Text DiehighScoreText;

    [Header("애니메이션 텍스트")]
    [SerializeField] private TMP_Text gameOverText;

    private Vector3 gameOverTextStartPos;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (gameOverText != null)
        {
            gameOverTextStartPos = gameOverText.transform.localPosition;
            gameOverText.gameObject.SetActive(true);
            Color color = gameOverText.color;
            color.a = 0f;
            gameOverText.color = color;
        }
    }

    public void StartGameOver()
    {
        if (DiecurrentScoreText != null)
            DiecurrentScoreText.text = $"{ScoreManager.instance.CurrentScore}";

        if (DiehighScoreText != null)
            DiehighScoreText.text = $"{ScoreManager.instance.HighScore}";

        GameManager.Instance?.SetGameOver(true);

        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        // 1. 배경 + 텍스트 동시에 시작
        yield return StartCoroutine(FadeInBackgroundWithText());

        // 2. 텍스트 위로 이동하며 사라지기
        yield return StartCoroutine(FadeOutGameOverText());

        // 3. 점수 패널 등장
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        yield return StartCoroutine(FadeInGameOverUI());
    }

    private IEnumerator FadeInBackgroundWithText()
    {
        float time = 0f;
        Color fadeColor = fadeImage.color;
        Color textColor = gameOverText.color;

        Vector3 startPos = gameOverTextStartPos;
        Vector3 midPos = startPos + new Vector3(0, 20f, 0); // 살짝 위로

        gameOverText.transform.localPosition = startPos;
        gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
        gameOverText.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);

            // 배경 페이드
            fadeColor.a = t;
            fadeImage.color = fadeColor;

            // 텍스트 페이드 + 이동
            gameOverText.transform.localPosition = Vector3.Lerp(startPos, midPos, t);
            textColor.a = t;
            gameOverText.color = textColor;

            yield return null;
        }

        gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
    }

    private IEnumerator FadeOutGameOverText()
    {
        float time = 0f;
        float duration = 1.0f;

        Vector3 startPos = gameOverText.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 80f, 0);
        Color textColor = gameOverText.color;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;

            gameOverText.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            textColor.a = Mathf.Lerp(1f, 0f, t);
            gameOverText.color = textColor;

            yield return null;
        }

        gameOverText.gameObject.SetActive(false);
    }

    private IEnumerator FadeInGameOverUI()
    {
        float time = 0f;
        canvasGroup.alpha = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeDuration;
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;
    }
}
