using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        // ✅ 플레이어랑 충돌 무시 설정
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider playerCol = player.GetComponent<Collider>();
            Collider bulletCol = GetComponent<Collider>();
            if (playerCol != null && bulletCol != null)
            {
                Physics.IgnoreCollision(bulletCol, playerCol);
            }
        }

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // ✅ 플레이어에게 맞음
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Me>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            return;
        }

        // ✅ 적에게 맞음
        if (other.CompareTag("EnemyText"))
        {
            var enemy = other.GetComponent<EnemyText3DAIController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.PlayDeathSFX();
            }

            Destroy(gameObject);
        }
    }
}