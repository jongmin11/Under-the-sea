using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 2f;
    public float lifeTime = 6f;
    public bool isDestructible = true;
    public int damage = 1;
    public GameObject vanishEffectPrefab;
    public enum BulletColorType { Orange, Pink }
    public BulletColorType bulletColorType = BulletColorType.Orange;
    [Header("HP Settings")]
    public int maxHP = 3;
    private int currentHP;


    private Vector3 moveDir = Vector3.down;

    void Start()
    {
        currentHP = maxHP;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        // ✅ Z값 플레이어에 맞추기
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.z = player.position.z;
            transform.position = pos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어 충돌 처리
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Me>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            if (isDestructible)
                Die();
        }

        // 플레이어 탄에 맞았을 때
        if (other.CompareTag("PlayerBullet"))
        {
            // 🛑 핑크 탄막이면 피격 무시
            if (bulletColorType == BulletColorType.Pink)
                return;

            Destroy(other.gameObject);

            if (isDestructible)
                TakeDamage(1); // 기본 1 대미지
        }
    }

    public void SetDirection(Vector3 dir)
    {
        moveDir = dir.normalized;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (vanishEffectPrefab != null)
        {
            GameObject fx = Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f); // 자동 제거
        }

        Destroy(gameObject); // 탄막 파괴
    }
}