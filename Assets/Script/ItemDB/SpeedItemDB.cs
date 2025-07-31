using UnityEngine;

// < summary >
// (������)�÷��̾�� �浹 �� ���� �ð� ���� �ӵ��� ��ȭ��ŵ�ϴ�.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SpeedItem : MonoBehaviour
{
    public enum SpeedType { SpeedUp, SpeedDown }

    [Header("���ǵ� ����")]
    public SpeedType type = SpeedType.SpeedUp;
    [Header("�ӵ� ���� ���� (��:1.5 == 50%)")]
    public float multiplier = 1.5f;
    [Tooltip("�ӵ� ���� ���� (��: 0.5 = ���� �ӵ�)")]
    public float speedDownMultiplier = 0.5f;
    [Header("���ӽð� �ʴ���")]
    public float duration = 3f;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Player")) return;

        //var movement = other.GetComponent<PlayerMove>(); // �÷��̾ũ��Ʈ
        //if (movement == null)
        //{
            //Debug.LogWarning("SpeedItem: PlayerMove ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            //return;

    //    var movement = other.GetComponent<PlayerMove>(); // �÷��̾ũ��Ʈ
    //    if (movement == null)
    //    {
    //        Debug.LogWarning("SpeedItem: PlayerMove ��ũ��Ʈ�� ã�� �� �����ϴ�.");
    //        return;
    //    }


    //    float effectMultiplier = (type == SpeedType.SpeedUp)
    //        ? multiplier
    //        : speedDownMultiplier;


        //movement.ApplySpeedEffect(effectMultiplier, duration);
        // Destroy(gameObject);
    }

    //    movement.ApplySpeedEffect(effectMultiplier, duration);
    //    Destroy(gameObject);
    //}

