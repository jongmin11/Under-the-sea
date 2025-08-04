using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민) 일정 시간마다 아이템을 생성하며, 폴리곤 포함 장애물과 절대 겹치지 않도록 검사합니다.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹 목록")]
    public GameObject[] itemPrefabs;

    [Header("스폰 범위 (플레이어 앞쪽 기준)")]
    public float forwardDistance = 5f;
    public float verticalRange = 2f;
    public float groundY = -3.5f;

    [Header("생성 타이밍")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("충돌 검사")]
    public LayerMask obstacleLayer;
    public Vector2 itemBoxSize = new Vector2(1f, 1f);
    public int maxSpawnAttempts = 10;

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("ItemSpawner: 'Player' 태그를 찾을 수 없습니다.");

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null || Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnItem();
            timer = 0f;
        }
    }

    void TrySpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 spawnPos = GetFrontSpawnPos();

            // 카메라 오른쪽 안이면 생성 안 함
            float cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x;
            if (spawnPos.x <= cameraRightEdge) return;

            // OverlapBox로 정확히 장애물과 겹치는지 검사
            bool blocked = Physics2D.OverlapBox(spawnPos, itemBoxSize, 0f, obstacleLayer);
            if (!blocked)
            {
                Instantiate(prefab, spawnPos, Quaternion.identity);
                return;
            }

        }
    }

    Vector2 GetFrontSpawnPos()
    {
        float x = player.position.x + forwardDistance;
        float y = groundY + Random.Range(0f, verticalRange);
        return new Vector2(x, y);
    }
}