using System.Collections;
using UnityEngine;

public class EnemyBulletShooter : MonoBehaviour
{
    public GameObject bulletPrefabOrange;
    public GameObject bulletPrefabPink;
    public bool usePinkBullet = false;

    public int bulletCount = 12;
    public float bulletInterval = 1.0f;
    public float bulletSpinSpeed = 20f;
    public float bulletAngleOffset = 0f;
    public int bulletDamage = 1;

    private bool isFiring = false;
    private bool isDead = false;

    public void StartShooting()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireRotatingBurst());
        }
    }

    public void StopShooting()
    {
        isDead = true;
    }

    IEnumerator FireRotatingBurst()
    {
        float angle = bulletAngleOffset;

        while (!isDead)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float a = angle + (360f / bulletCount) * i;
                FireBulletAtAngle(a);
            }

            angle += bulletSpinSpeed;
            yield return new WaitForSeconds(bulletInterval);
        }
    }

    void FireBulletAtAngle(float angle)
    {
        GameObject prefab = usePinkBullet ? bulletPrefabPink : bulletPrefabOrange;
        if (prefab == null) return;

        Vector3 pos = transform.position;

        // ✅ 플레이어 Z 맞추기
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            pos.z = player.position.z;
        }

        Quaternion rot = Quaternion.Euler(0, 0, angle);
        GameObject bullet = Instantiate(prefab, pos, rot);

        var renderer = bullet.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.renderQueue = 2000;

        EnemyBullet b = bullet.GetComponent<EnemyBullet>();
        if (b != null)
        {
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            b.SetDirection(dir);
            b.damage = bulletDamage;
        }
    }
}