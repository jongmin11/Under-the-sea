using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (������)�÷��̾ ���ΰ� �浹 �� ������ ������Ű�� ������ ��ũ��Ʈ�Դϴ�.
/// ScoreManager�� �����ؾ� �ϸ�, �÷��̾�� "Player" �±װ� �����Ǿ� �־�� �մϴ�.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CoinItemDB : MonoBehaviour
{
    [Header("���� ����")]
    [Tooltip("ȹ�� �� ������ ������")]
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
            Debug.LogWarning("CoinItemDB: ScoreManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }

        Destroy(gameObject);
    }
}
