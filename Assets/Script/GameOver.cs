using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("게임 오버 UI")]
    [Tooltip("게임 오버 시 활성화될 UI 패널 오브젝트")]
    [SerializeField] private GameObject gameOverPanelObject;

    [Header("페이드 설정")]
    [Tooltip("화면을 어둡게 덮는 Image (검정색)")]
    [SerializeField] private Image fadeImage;

    [Tooltip("페이드 아웃에 걸리는 시간 (초)")]
    [SerializeField] public float fadeDuration = 1.5f;

    /// <summary>
    /// (한종민)시작 시, 페이드 이미지와 게임오버 패널을 초기화합니다.
    /// </summary>
    void Awake()
    {
        GameObject bg = GameObject.Find("BG");
        if (bg == null) 
            return;

        gameOverPanelObject = bg.transform.Find("GameOverPanel")?.gameObject;
        if (gameOverPanelObject == null) 
            return; 

        Transform imageTransform = gameOverPanelObject.transform.Find("Image");
        if (imageTransform == null) 
            return;

        fadeImage = imageTransform.GetComponent<Image>();
        if (fadeImage == null) 
            return;

        
        Color start = fadeImage.color;
        start.a = 0f; // 페이드 이미지 알파 0으로 시작
        fadeImage.color = start;
    }

    /// <summary>
    /// (한종민)외부에서 호출되는 게임오버 시작 함수입니다. 페이드 연출을 시작합니다.
    /// </summary>
    public void StartGameOver()
    {
        gameOverPanelObject.SetActive(true);
        StartCoroutine(FadeOutAndShow());
    }

    /// <summary>
    /// (한종민)페이드 아웃 후 게임 오버 UI를 표시하는 코루틴입니다.
    /// </summary>
    IEnumerator FadeOutAndShow()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
