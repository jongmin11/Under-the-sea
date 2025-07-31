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

    private bool isPaused = false;
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
    /// (한종민)일시정지를 토글합니다.  
    /// 시간 정지 및 UI 전환 처리를 수행합니다.
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
        SceneManager.LoadScene("MainGameScene");
    }

}
