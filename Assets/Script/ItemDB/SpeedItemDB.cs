using UnityEngine;

// < summary >
// (한종민)플레이어와 충돌 시 일정 시간 동안 속도를 변화시킵니다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SpeedItem : MonoBehaviour
{
    public enum SpeedType { SpeedUp, SpeedDown }

    [Header("스피드 설정")]
    public SpeedType type = SpeedType.SpeedUp;
    [Header("속도 증가 배율 (예:1.5 == 50%)")]
    public float multiplier = 1.5f;
    [Tooltip("속도 감소 배율 (예: 0.5 = 절반 속도)")]
    public float speedDownMultiplier = 0.5f;
    [Header("지속시간 초단위")]
    public float duration = 3f;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    var movement = other.GetComponent<PlayerMove>(); // 플레이어스크립트
    //    if (movement == null)
    //    {
    //        Debug.LogWarning("SpeedItem: PlayerMove 스크립트를 찾을 수 없습니다.");
    //        return;
    //    }

    //    float effectMultiplier = (type == SpeedType.SpeedUp)
    //        ? multiplier
    //        : speedDownMultiplier;

    //    movement.ApplySpeedEffect(effectMultiplier, duration);
    //    Destroy(gameObject);
    //}
}

