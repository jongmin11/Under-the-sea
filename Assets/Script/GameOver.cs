using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 사망 시, 게임 오버 연출을 담당하는 컨트롤러
/// - Game Over 텍스트 애니메이션
/// - 검정 페이드 배경
/// - 점수창 UI 슬라이드 연출
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("UI 요소")]
    [Tooltip("게임 오버 시 표시할 패널 (점수 및 버튼 포함)")]
    [SerializeField] private GameObject gameOverPanel;
    [Tooltip("gameOverPanel에 부착된 CanvasGroup (페이드 연출용)")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("페이드 이미지")]
    [SerializeField] private Image fadeImage;

    [Header("페이드 설정")]
    [Tooltip("페이드 인/아웃에 소요될 시간")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("점수 UI")]
    [Tooltip("현재 점수를 표시할 텍스트")]
    [SerializeField] private TMP_Text DieCurrentScore;
    [Tooltip("최고 점수를 표시할 텍스트")]
    [SerializeField] private TMP_Text DieHighScore;

    [Header("게임오버 텍스트")]
    [SerializeField] private TMP_Text gameOverText;

    private Vector3 originalPanelPos;
    private Vector3 gameOverTextStartPos;

    /// <summary>
    /// 모든 UI 초기화. 텍스트/패널/페이드 상태 및 위치 설정
    /// </summary>
    private void Awake()
    {
        if (gameOverPanel != null) // 점수 패널 숨기고 위치 저장
        {
            gameOverPanel.SetActive(false);
            originalPanelPos = gameOverPanel.transform.localPosition;
        }

        if (canvasGroup != null)  // 패널 CanvasGroup 투명하게 초기화
            canvasGroup.alpha = 0f;

        if (fadeImage != null)   // 검정 페이드 이미지 설정
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (gameOverText != null) // GameOver 텍스트 위치 및 알파 초기화
        {
            gameOverTextStartPos = gameOverText.transform.localPosition;
            Color c = gameOverText.color;
            c.a = 0f;
            gameOverText.color = c;
        }
    }

    /// <summary>
    /// 게임 오버 연출 전체 시작
    /// - 점수 적용
    /// - 연출 순차 실행 (텍스트 → 배경 → 패널)
    /// </summary>
    public void StartGameOver()
    {
        GameManager.Instance?.SetGameOver(true);


        if (DieCurrentScore != null) 
            DieCurrentScore.text = $"{ScoreManager.instance.CurrentScore}"; // 점수 텍스트 갱신

        if (DieHighScore != null)
            DieHighScore.text = $"{ScoreManager.instance.HighScore}";

        StartCoroutine(FadeInSequence()); // 전체 연출 실행
    }

    /// <summary>
    /// 전체 연출 시퀀스 실행: 
    /// 1. GameOver 텍스트 애니메이션
    /// 2. 배경 페이드 인
    /// 3. 점수 패널 슬라이드+페이드 인
    /// </summary>
    private IEnumerator FadeInSequence()
    {
        yield return StartCoroutine(FadeInAndOutGameOverText());      // 1. 텍스트 등장
        yield return StartCoroutine(FadeInBackground());        // 2. 배경
        yield return StartCoroutine(FadeInGameOverPanel());     // 3. 점수창
    }

    /// <summary>
    /// GameOver 텍스트를 위로 떠오르게 하며 페이드 인 → 정지 → 사라짐
    /// </summary>
    private IEnumerator FadeInAndOutGameOverText()
    {
        float fadeInTime = 1.0f;
        float pauseTime = 0.3f;
        float fadeOutTime = 0.6f;
        float moveUpAmount = 80f;

        gameOverText.gameObject.SetActive(true);

        Vector3 startPos = gameOverTextStartPos;
        Vector3 middlePos = startPos + new Vector3(0, moveUpAmount * 0.3f, 0);
        Vector3 endPos = startPos + new Vector3(0, moveUpAmount, 0);

        gameOverText.transform.localPosition = startPos;

        // 알파 0으로 시작 (색은 그대로 유지)
        {
            Color c = gameOverText.color;
            c.a = 0f;
            gameOverText.color = c;
        }

        // 1. 페이드 인 + 살짝 위로 이동
        float time = 0f;
        while (time < fadeInTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeInTime;

            gameOverText.transform.localPosition = Vector3.Lerp(startPos, middlePos, t);

            Color c = gameOverText.color;
            c.a = Mathf.Lerp(0f, 1f, t); // 알파 보간
            gameOverText.color = c;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(pauseTime);  // 2. 멈춤

        // 3. 페이드 아웃 + 더 위로 이동
        time = 0f;
        while (time < fadeOutTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeOutTime;

            gameOverText.transform.localPosition = Vector3.Lerp(middlePos, endPos, t);

            Color c = gameOverText.color;
            c.a = Mathf.Lerp(1f, 0f, t); // 알파 감소
            gameOverText.color = c;

            yield return null;
        }

        gameOverText.gameObject.SetActive(false);  // 오브젝트 비활성화
    }

    /// <summary>
    /// 검정 배경 이미지 페이드 인
    /// </summary>
    private IEnumerator FadeInBackground()
    {
        float time = 0f;
        Color fadeColor = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            fadeColor.a = t; // 알파 증가
            fadeImage.color = fadeColor;
            yield return null;
        }
    }

    /// <summary>
    /// GameOverPanel을 아래에서 위로 슬라이드 + 페이드 인
    /// </summary>
    private IEnumerator FadeInGameOverPanel()
    {
        float time = 0f;
        float duration = fadeDuration;

        Vector3 fromPos = originalPanelPos + new Vector3(0, -100f, 0);
        gameOverPanel.transform.localPosition = fromPos;
        gameOverPanel.SetActive(true);
        canvasGroup.alpha = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;


            gameOverPanel.transform.localPosition = Vector3.Lerp(fromPos, originalPanelPos, t); // 위치 보간 + 알파 보간
            canvasGroup.alpha = t;

            yield return null;
        }

        gameOverPanel.transform.localPosition = originalPanelPos;
        canvasGroup.alpha = 1f;

        Time.timeScale = 0f; // 게임 정지
    }
}