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
    [Tooltip("게임오버 패널 (전체 UI 포함)")]
    [SerializeField] private GameObject gameOverPanel;

    [Tooltip("게임오버 패널에 붙은 CanvasGroup")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("페이드 이미지")]
    [Tooltip("검정 배경 이미지 (페이드용)")]
    [SerializeField] private Image fadeImage;

    [Header("페이드 설정")]
    [Tooltip("페이드 아웃 시간 (초)")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("점수 UI")]
    [Tooltip("현재 점수를 표시할 TMP 텍스트")]
    [SerializeField] private TMP_Text DiecurrentScoreText;

    [Tooltip("최고 점수를 표시할 TMP 텍스트")]
    [SerializeField] private TMP_Text DiehighScoreText;

    /// <summary>
    /// 시작 시 게임오버 UI를 꺼두고, 알파를 0으로 설정합니다.
    /// </summary>
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
    }

    /// <summary>
    /// 외부에서 호출되는 게임오버 진입 함수입니다.
    /// </summary>
    public void StartGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (DiecurrentScoreText != null)
            DiecurrentScoreText.text = $"{ScoreManager.instance.CurrentScore}";

        if (DiehighScoreText != null)
            DiehighScoreText.text = $"{ScoreManager.instance.HighScore}";

        StartCoroutine(FadeInGameOverUI());
    }

    /// <summary>
    /// CanvasGroup의 알파값을 서서히 증가시켜 전체 UI를 페이드인합니다.
    /// </summary>
    private IEnumerator FadeInGameOverUI()
    {
        float time = 0f;

        Color fadeColor = fadeImage != null ? fadeImage.color : Color.black;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(time / fadeDuration);

            if (canvasGroup != null)
                canvasGroup.alpha = alpha;

            if (fadeImage != null)
            {
                fadeColor.a = alpha;
                fadeImage.color = fadeColor;
            }

            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        if (fadeImage != null)
        {
            fadeColor.a = 1f;
            fadeImage.color = fadeColor;
        }

        
        

        Time.timeScale = 0f;
    }
}
