using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (������) �÷��̾�� �浹 �� ü���� ȸ����ŵ�ϴ�.
/// PlayerHealth ������Ʈ�� ���� �����մϴ�.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HealItemDB : MonoBehaviour
{
    [Header("ȸ���� ����")]
    [Tooltip("ȸ���� (��: 20 = ü�� +20)")]
    public int healAmount = 20;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    var playerHealth = other.GetComponent<PlayerHealth>(); // �÷��̾� ü��
    //    if (playerHealth == null)
    //    {
    //        Debug.LogWarning("HealItemDB: PlayerHealth ������Ʈ�� ã�� �� �����ϴ�.");
    //        return;
    //    }

    //    playerHealth.Heal(healAmount);
    //    Destroy(gameObject);
    //}
}
