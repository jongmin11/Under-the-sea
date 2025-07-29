using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민)일정 시간마다 아이템을 생성하며, 장애물과 겹치지 않도록 충돌 검사를 진행합니다.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹 목록")]
    public GameObject[] itemPrefabs;

    [Header("생성 영역 설정")]
    public Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 3f);

    [Header("생성 타이밍")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("충돌 검사")]
    [Tooltip("장애물 레이어")]
    public LayerMask obstacleLayer;

    [Tooltip("겹침 검사 반지름")]
    public float overlapCheckRadius = 0.5f;

    void Update()
    {
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

        // 위치 랜덤 생성
        Vector2 position = GetRandomPosition();

        // 충돌 검사
        bool overlaps = Physics2D.OverlapCircle(position, overlapCheckRadius, obstacleLayer);
        if (overlaps) return; // 장애물과 겹치면 생성 취소

        // 아이템 종류 랜덤 선택
        int index = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[index], position, Quaternion.identity);
    }

    Vector2 GetRandomPosition() 
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }
}
