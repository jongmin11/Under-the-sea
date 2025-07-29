using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// 플레이어가 이 아이템과 충돌하면 일정 시간 동안 속도 변화 효과를 적용합니다.
/// 아이템은 충돌 후 사라지며, 플레이어는 반드시 ApplySpeedEffect(float, float) 메서드를 가져야 합니다.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ItemDB : MonoBehaviour
{
   public enum ItemType //지금은 스피드업/다운만 있음
   {  
        SpeedUp,
        SpeedDOWN
   }
   [Header("아이템 설정")]
   [Tooltip("아이템 효과 종류")]
   public ItemType type = ItemType.SpeedUp;

   [Tooltip("적용할 속도 배율 (예: 1.5f = 50% 증가)")]
   public float multiplier = 1.5f;

   [Tooltip("효과 지속 시간 (초)")]
   public float duration = 3f;
   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
          var movement = other.GetComponent<PlayerMove>(); // 또는 다른 스크립트
          if (movement = null)
          {
                Debug.LogWarning("ItemDB: PlayerMove 스크립트가 플레이어에 없습니다.");
                return;
          }

            // 속도 효과 적용
            float effectMultiplier = (type == ItemType.SpeedUp)? multiplier :1f / multiplier;
            movement.ApplySpeedEffect(effectMultiplier, duration);

           Destroy(gameObject);

      }
   }
}
