using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민)플레이어가 코인과 충돌 시 점수를 증가시키는 아이템 스크립트입니다.
/// ScoreManager가 존재해야 하며, 플레이어는 "Player" 태그가 설정되어 있어야 합니다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CoinItemDB : MonoBehaviour
{
    [Header("코인 설정")]
    [Tooltip("획득 시 증가할 점수량")]
    public int scoreValue = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }
        else
        {
            Debug.LogWarning("CoinItemDB: ScoreManager 인스턴스를 찾을 수 없습니다.");
        }

        Destroy(gameObject);
    }
}
