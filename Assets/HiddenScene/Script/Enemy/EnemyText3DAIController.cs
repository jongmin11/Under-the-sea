using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyText3DAIController : MonoBehaviour
{
    public TextMesh textMesh;
    public Transform player;

    public GameObject bulletPrefabOrange;
    public GameObject bulletPrefabPink;
    public bool usePinkBullet = false;

    private EnemyTextAIType aiType = EnemyTextAIType.Straight;
    private Vector3 moveDirection;
    private Vector3 centerPoint;
    private float angle = 0f;
    private float speed = 2f;
    private float elapsed = 0f;
    private bool isDead = false;
    private int hp = 10;
    public AudioSource audioSource;
    public AudioClip deathClip;
    private enum Phase { Falling, AIStart }
    private Phase currentPhase = Phase.Falling;
    private Vector3 fallTarget;
    private float stopDistance = 0.1f;

    private bool rushStarted = false;
    private float rushDelay = 1.5f;

    private float pingpongRange = 2.5f;
    private float pingpongSpeed = 0.5f;
    private float pingpongStartX;
    private float pingpongOffset;
    private float baseY;

    private EnemyBulletShooter shooter;
    private bool bulletStarted = false;

    private float lastHitTime = -1f;
    private float hitCooldown = 0.05f;
    private EnemyTextHitEffect hitEffect;
    public GameObject explosionEffectPrefab;

    public void PlayDeathSFX()
    {
        if (audioSource != null && deathClip != null)
        {
            // 🔊 새로운 오브젝트 생성해서 그 안에서 소리 재생
            GameObject audioObj = new GameObject("Temp_DeathSFX");
            AudioSource tempAudio = audioObj.AddComponent<AudioSource>();
            tempAudio.clip = deathClip;
            tempAudio.volume = audioSource.volume;         // 기존 볼륨 유지
            tempAudio.outputAudioMixerGroup = audioSource.outputAudioMixerGroup; // 믹서도 복사 (필요시)
            tempAudio.Play();

            // 소리 끝나면 제거
            Destroy(audioObj, deathClip.length);
        }
    }
    void Start()
    {
        hitEffect = GetComponentInChildren<EnemyTextHitEffect>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Setup(string content, float fontSize, Color color, float speedValue, float angleDeg, int hpValue, EnemyTextAIType ai, Transform playerRef)
    {
        textMesh = GetComponentInChildren<TextMesh>();

        if (textMesh == null)
        {
            Debug.LogError("❌ TextMesh 없음");
            return;
        }

        shooter = GetComponent<EnemyBulletShooter>();
        if (shooter != null)
        {
            shooter.usePinkBullet = (transform.position.x > 0f);
        }

        textMesh.text = content;
        textMesh.fontSize = 64;
        textMesh.characterSize = fontSize;
        textMesh.color = color;

        player = playerRef;

        // ✅ 플레이어 Z값 따라가기
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.z = player.position.z;
            transform.position = pos;
        }

        speed = speedValue;
        hp = hpValue;
        aiType = ai;

        float rad = angleDeg * Mathf.Deg2Rad;
        moveDirection = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

        centerPoint = transform.position;
        fallTarget = transform.position + Vector3.down * 5f;

        if (aiType == EnemyTextAIType.Straight)
        {
            pingpongStartX = transform.position.x;
            pingpongOffset = Random.Range(0f, 10f);
            baseY = fallTarget.y + Random.Range(-0.5f, 1.0f);
        }

        currentPhase = Phase.Falling;

        StartCoroutine(LateColliderUpdate());
    }

    IEnumerator LateColliderUpdate()
    {
        yield return null;
        UpdateColliderToFitText();
    }

    void UpdateColliderToFitText()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (textMesh == null || col == null) return;

        Renderer rend = textMesh.GetComponent<Renderer>();
        if (rend == null) return;

        Vector3 size = rend.bounds.size;
        size.z = 20f; // 💥 여기다 Z축 빵 키워!
        col.size = size;

        col.center = rend.bounds.center - transform.position;
        col.isTrigger = true;
    }

    void Update()
    {
        if (isDead) return;
        elapsed += Time.deltaTime;

        // ✅ 계속해서 플레이어 Z값 따라가기
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.z = player.position.z;
            transform.position = pos;
        }

        if (currentPhase == Phase.Falling)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallTarget, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, fallTarget) <= stopDistance)
            {
                currentPhase = Phase.AIStart;
                elapsed = 0f;
                centerPoint = transform.position;

                if (aiType == EnemyTextAIType.Straight)
                {
                    pingpongStartX = transform.position.x;
                }
            }
            return;
        }

        if (currentPhase == Phase.AIStart)
        {
            if (!bulletStarted)
            {
                bulletStarted = true;
                if (shooter != null)
                    shooter.StartShooting();
            }

            if (aiType == EnemyTextAIType.Straight)
            {
                UpdateStraightPingpong();
                return;
            }

            switch (aiType)
            {
                case EnemyTextAIType.FollowPlayer:
                    if (player != null)
                    {
                        Vector3 dir = (player.position - transform.position).normalized;
                        transform.position += new Vector3(dir.x, dir.y, 0f) * speed * Time.deltaTime;
                    }
                    break;

                case EnemyTextAIType.Orbit:
                    angle += 60f * Time.deltaTime;
                    float rad = angle * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 2f;
                    transform.position = centerPoint + offset;
                    break;

                case EnemyTextAIType.ZigZag:
                    float offsetX = Mathf.Sin(elapsed * 5f) * 0.5f;
                    transform.position += new Vector3(offsetX, -speed * Time.deltaTime, 0f);
                    break;

                case EnemyTextAIType.RushIn:
                    if (!rushStarted && elapsed >= rushDelay)
                    {
                        rushStarted = true;
                        if (player != null)
                            moveDirection = (player.position - transform.position).normalized;
                    }

                    if (rushStarted)
                        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0f) * speed * 3f * Time.deltaTime;
                    break;
            }

            if (transform.position.magnitude > 200f)
                Destroy(gameObject);
        }
    }

    void UpdateStraightPingpong()
    {
        float x = Mathf.PingPong((Time.time + pingpongOffset) * pingpongSpeed, pingpongRange * 2f) - pingpongRange;
        float y = baseY;
        float z = player != null ? player.position.z : transform.position.z;
        transform.position = new Vector3(pingpongStartX + x, y, z);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        hp -= dmg;

        // ✅ 쉐이크 효과
        if (hitEffect != null)
        {
            hitEffect.Play(0.15f, 0.2f); // 흔들림: 0.15초 / 강도 0.2
        }

        if (hp <= 0)
        {
            isDead = true;

            // ✅ 탄막 중지
            if (shooter != null)
                shooter.StopShooting();

            // ✅ 파편 이펙트 (있을 경우)
            if (explosionEffectPrefab != null)
            {
                GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(fx, 2f); // 파티클 2초 후 삭제
            }

            Destroy(gameObject);
        }
    }

    public void ForceKill()
    {
        if (isDead) return;
        isDead = true;

        if (shooter != null)
            shooter.StopShooting();

        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }
}