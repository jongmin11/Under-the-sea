using UnityEngine;
using System.Collections;
public class Me : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireInterval = 0.3f;
    public float bulletSpeed = 10f;

    private Rigidbody rb;
    private Vector3 movement;
    private float fireTimer = 0f;

    private bool isInvincible = false;
    private float invincibleDuration = 0.1f;

    public LayerMask bulletLayer;
    public LayerMask enemyTextLayer;

    public int maxHP = 3;
    private int currentHP;
    public static int deathCount = 0;

    private MeHitShake hitShake;
    private bool isDead = false;

    private CameraShake cameraShake;
    public GameObject hitEffectPrefab;

    public float disappearDuration = 1.5f; // 서서히 사라지는 시간
    private Renderer[] renderers;
    private bool isDisappearing = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitShake = GetComponentInChildren<MeHitShake>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        currentHP = maxHP;
        renderers = GetComponentsInChildren<Renderer>();
        
    }

    void Update()
    {
        if (isDead) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        movement = new Vector3(h, v, 0f).normalized;

        RotateToMouse();

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.K))
            Die();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void RotateToMouse()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = 10f; // 카메라 클립 거리
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        Vector3 lookDir = (mouseWorld - transform.position).normalized;
        lookDir.z = 0f;

        if (lookDir != Vector3.zero)
            rb.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        transform.position = pos;
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        if (rbBullet != null)
        {
            Vector3 shootDir = firePoint.up.normalized;
            rbBullet.velocity = shootDir * bulletSpeed;

        }
        GetComponent<PlayerSFX>()?.PlayShootSFX();
    }

    public void TakeDamage(int dmg)
    {
        if (isDead || isInvincible) return;

        currentHP -= dmg;

        GetComponent<PlayerSFX>()?.PlayHitSFX();
        if (hitShake != null) hitShake.Play();
        if (cameraShake != null) cameraShake.Shake(0.2f, 0.1f);
        if (hitEffectPrefab != null)
        {
            GameObject fx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // 반경 5f 안에 있는 탄막 제거
        float radius = 5f;
        Collider[] bullets = Physics.OverlapSphere(transform.position, radius, bulletLayer);
        foreach (var b in bullets)
        {
            EnemyBullet eb = b.GetComponent<EnemyBullet>();
            if (eb != null) eb.Die(); // 탄막 제거
        }

        Collider[] texts = Physics.OverlapSphere(transform.position, radius, enemyTextLayer);
        foreach (var t in texts)
        {
            EnemyText3DAIController ctrl = t.GetComponent<EnemyText3DAIController>();
            if (ctrl != null) ctrl.ForceKill(); // 텍스트 제거
        }

        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    private IEnumerator FadeOutAndDisappear()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            foreach (var r in renderers)
            {
                if (r.material.HasProperty("_Color"))
                {
                    Color c = r.material.color;
                    c.a = alpha;
                    r.material.color = c;
                }
            }

            yield return null;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<PlayerSFX>()?.PlayDeathSFX();
        deathCount++;

        Time.timeScale = 0f;

        var fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
            fader.FadeIn(Color.white, 2.5f);

        var prompt = FindObjectOfType<PromptTextController>();
        if (prompt != null)
            prompt.StartDialogueAfterDeath(deathCount);


    }

    public void Revive()
    {
        isDead = false;
        Time.timeScale = 1f;

        // 체력 회복
        currentHP = maxHP;

        // 주변 탄막 제거
        float radius = 5f;

        Collider[] bullets = Physics.OverlapSphere(transform.position, radius, bulletLayer);
        foreach (var b in bullets)
        {
            EnemyBullet eb = b.GetComponent<EnemyBullet>();
            if (eb != null) eb.Die();
        }

        Collider[] texts = Physics.OverlapSphere(transform.position, radius, enemyTextLayer);
        foreach (var t in texts)
        {
            EnemyText3DAIController ctrl = t.GetComponent<EnemyText3DAIController>();
            if (ctrl != null) ctrl.ForceKill();
        }
    }

    public void AppearAndRevive()
    {
        StartCoroutine(FadeInAndRevive());
    }

    private IEnumerator FadeInAndRevive()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        isDead = false;
        Time.timeScale = 1f;
        enabled = true;
        EnableAllComponents();
    }

    private void EnableAllComponents()
    {
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            if (comp != this) // Me.cs 자신 제외
                comp.enabled = true;
        }
    }
}
