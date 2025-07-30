using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (한종민) 게임의 현재 점수 및 최고 점수를 관리하는 싱글톤 클래스입니다.
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
    /// (한종민) 점수를 추가합니다. 최고 점수를 자동으로 갱신합니다.
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
    /// (한종민) 현재 점수를 0으로 초기화합니다.  
    /// 최고 점수는 그대로 유지됩니다.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
    }

    /// <summary>
    /// (한종민) 현재 최고 점수를 로컬 저장소(PlayerPrefs)에 저장합니다.
    /// </summary>
    void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// (한종민) 로컬 저장소(PlayerPrefs)에서 최고 점수를 불러옵니다.
    /// 값이 없을 경우 0으로 초기화됩니다.
    /// </summary>
    void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey);
    }
}
