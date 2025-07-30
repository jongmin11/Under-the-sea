using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민) 플레이어와 충돌 시 체력을 회복시킵니다.
/// PlayerHealth 컴포넌트에 직접 접근합니다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HealItemDB : MonoBehaviour
{
    [Header("회복량 설정")]
    [Tooltip("회복량 (예: 20 = 체력 +20)")]
    public int healAmount = 20;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    var playerHealth = other.GetComponent<PlayerHealth>(); // 플레이어 체력
    //    if (playerHealth == null)
    //    {
    //        Debug.LogWarning("HealItemDB: PlayerHealth 컴포넌트를 찾을 수 없습니다.");
    //        return;
    //    }

    //    playerHealth.Heal(healAmount);
    //    Destroy(gameObject);
    //}
}
