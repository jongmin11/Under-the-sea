using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// (한종민)게임의 일시정지, 재개, 재시작, 메인화면 이동 기능을 담당하는 매니저 클래스입니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanal;
    [SerializeField] private GameObject pauseButton;

    [Header("ESC 키 효과음")]
    [SerializeField] private AudioClip pauseSfxClip;

    private bool isPaused = false;

    private bool isGameOver = false;
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// (한종민)GameManager 인스턴스를 초기화합니다.  
    /// 중복된 경우 자동으로 삭제됩니다.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// (한종민) ESC 키 입력으로 일시정지를 토글하고, 효과음을 재생합니다.
    /// 게임 오버 상태일 경우 무시됩니다.
    /// </summary>
    private void Update()
    {
        
        // ESC 키로 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver)
                return;

            if (pauseSfxClip != null)
            {
                // 현재 GameObject에 AudioSource 없으면 붙이기
                if (!TryGetComponent(out AudioSource src))
                    src = gameObject.AddComponent<AudioSource>();

                src.PlayOneShot(pauseSfxClip);
            }

            OnClickPauseToggle();
        }
    }

    /// <summary>
    /// (한종민) 일시정지를 토글하여 Time.timeScale을 0 또는 1로 설정하고,
    /// 일시정지 UI 및 버튼의 활성 상태를 전환합니다.
    /// </summary>
    public void OnClickPauseToggle()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0.0f : 1.0f;
        pausePanal.SetActive(isPaused);

        pauseButton.SetActive(!isPaused);
    }

    /// <summary>
    /// (한종민)일시정지를 해제하고 게임을 재개합니다.
    /// </summary>
    public void OnClickResume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;

        pausePanal.SetActive(false);
        pauseButton.SetActive(true);
    }

    /// <summary>
    /// (한종민)현재 씬을 다시 로드하여 게임을 재시작합니다.
    /// </summary>
    public void OnClickRestart()
    {
        Time.timeScale = 1.0f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// (한종민)메인 메뉴 씬으로 이동합니다.
    /// </summary>
    public void OnClickGoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// (한종민) ESC 키 입력 및 일시정지 기능을 게임 오버 시 비활성화합니다.
    /// </summary>
    /// <param name="value">게임 오버 상태 여부</param>
    public void SetGameOver(bool value)
    {
        isGameOver = value;
    }
}
