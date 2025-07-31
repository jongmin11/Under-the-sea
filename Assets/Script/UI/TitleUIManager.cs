using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public AudioSource sfxPlayer;           // ��ư ȿ���� �����
    public AudioClip clickSound;            // ��ư Ŭ�� �Ҹ�
    public string sceneToLoad = "MainGameScene"; // �ε��� �� �̸�

    public void StartGame()
    {
        // ȿ���� ���
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

