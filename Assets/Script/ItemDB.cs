using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// �÷��̾ �� �����۰� �浹�ϸ� ���� �ð� ���� �ӵ� ��ȭ ȿ���� �����մϴ�.
/// �������� �浹 �� �������, �÷��̾�� �ݵ�� ApplySpeedEffect(float, float) �޼��带 ������ �մϴ�.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ItemDB : MonoBehaviour
{
   public enum ItemType //������ ���ǵ��/�ٿ ����
   {  
        SpeedUp,
        SpeedDOWN
   }
   [Header("������ ����")]
   [Tooltip("������ ȿ�� ����")]
   public ItemType type = ItemType.SpeedUp;

   [Tooltip("������ �ӵ� ���� (��: 1.5f = 50% ����)")]
   public float multiplier = 1.5f;

   [Tooltip("ȿ�� ���� �ð� (��)")]
   public float duration = 3f;
   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
          var movement = other.GetComponent<PlayerMove>(); // �Ǵ� �ٸ� ��ũ��Ʈ
          if (movement = null)
          {
                Debug.LogWarning("ItemDB: PlayerMove ��ũ��Ʈ�� �÷��̾ �����ϴ�.");
                return;
          }

            // �ӵ� ȿ�� ����
            float effectMultiplier = (type == ItemType.SpeedUp)? multiplier :1f / multiplier;
            movement.ApplySpeedEffect(effectMultiplier, duration);

           Destroy(gameObject);

      }
   }
}
