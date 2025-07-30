using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (������) ������ ���� ���� �� �ְ� ������ �����ϴ� �̱��� Ŭ�����Դϴ�.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    public int CurrentScore { get; private set; } = 0;
    
    public int HighScore { get; private set; } = 0;

    const string HighScoreKey = "HighScore";

    public Text TestScore;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// (������) ������ �߰��մϴ�. �ְ� ������ �ڵ����� �����մϴ�.
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        CurrentScore += amount;

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            SaveHighScore();
        }

        if (TestScore != null)
        {
            TestScore.text = CurrentScore.ToString();
        }

    }

    /// <summary>
    /// (������) ���� ������ 0���� �ʱ�ȭ�մϴ�.  
    /// �ְ� ������ �״�� �����˴ϴ�.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
    }

    /// <summary>
    /// (������) ���� �ְ� ������ ���� �����(PlayerPrefs)�� �����մϴ�.
    /// </summary>
    void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// (������) ���� �����(PlayerPrefs)���� �ְ� ������ �ҷ��ɴϴ�.
    /// ���� ���� ��� 0���� �ʱ�ȭ�˴ϴ�.
    /// </summary>
    void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey);
    }
}
