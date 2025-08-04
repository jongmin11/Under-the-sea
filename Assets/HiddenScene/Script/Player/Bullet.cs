using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 1f;

    private void Start()
    {
        //  레이어 지정 (PlayerBullet)
        gameObject.layer = LayerMask.NameToLayer("PlayerBullet");

        // 일정 시간 뒤 삭제
        Destroy(gameObject, lifeTime);

        //  충돌 무시 설정 (Player, Ally)
        IgnorePlayerAndAllies();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyText"))
        {
            EnemyText3DAIController enemy = other.GetComponent<EnemyText3DAIController>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); 
            }

            Destroy(gameObject);
        }
    }

    private void IgnorePlayerAndAllies()
    {
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

        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject ally in allies)
        {
            Collider allyCol = ally.GetComponent<Collider>();
            Collider bulletCol = GetComponent<Collider>();
            if (allyCol != null && bulletCol != null)
            {
                Physics.IgnoreCollision(bulletCol, allyCol);
            }
        }
    }
}