using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public AudioSource sfxPlayer;           // 버튼 효과음 재생기
    public AudioClip clickSound;            // 버튼 클릭 소리
    public string sceneToLoad = "MainGameScene"; // 로드할 씬 이름

    public void StartGame()
    {
        // 효과음 재생
        if (sfxPlayer && clickSound)
        {
            sfxPlayer.PlayOneShot(clickSound);
        }


        Invoke("LoadNextScene", 0.3f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

