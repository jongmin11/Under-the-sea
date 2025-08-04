using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyFollower : MonoBehaviour
{
    public Transform centerTarget;
    public float radius = 2f;
    public float rotationSpeed = 50f;
    private float angle;

    void Update()
    {
        if (centerTarget == null) return;

        angle += rotationSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.position = centerTarget.position + offset;
    }

    public void MoveToPlayer(Vector3 playerPos)
    {
        StartCoroutine(MoveRoutine(playerPos));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        float t = 0f;
        Vector3 start = transform.position;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }
    }

    public void StartOrbit(Transform player, float radius, float speed)
    {
        centerTarget = player;
        this.radius = radius;
        this.rotationSpeed = speed;
        angle = Random.Range(0f, 360f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}