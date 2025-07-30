using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민)일정 시간마다 아이템을 생성하며, 장애물과 겹치지 않도록 충돌 검사를 진행합니다.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹 목록")]
    [Tooltip("생성할 아이템 프리팹 배열 (코인, 스피드 아이템 등)")]
    public GameObject[] itemPrefabs;

    [Header("스폰 범위 (플레이어 앞쪽 기준)")]
    [Tooltip("플레이어 기준 앞으로 몇 유닛 떨어진 곳에 생성할지")]
    public float forwardDistance = 5f;
    [Tooltip("플레이어 기준 위아래로 얼마나 퍼뜨릴지")]
    public float verticalRange = 2f;

    [Header("생성 타이밍")]
    [Tooltip("아이템 생성 간격 (초 단위)")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("충돌 검사")]
    [Tooltip("겹치면 생성 안 할 장애물의 레이어 마스크")]
    public LayerMask obstacleLayer;

    [Tooltip("겹침 체크를 위한 원형 반지름")]
    public float overlapCheckRadius = 0.5f;

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("ItemSpawner: 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // 메인 카메라 참조
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawnItem();
            timer = 0f;
        }
    }

    /// <summary>
    /// (한종민)아이템을 생성 가능한 위치에 스폰하고, 조건에 맞지 않으면 생성하지 않습니다.
    /// - 카메라 화면 안이면 생성 안 함
    /// - 장애물과 겹치면 생성 안 함
    /// </summary>
    void TrySpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        Vector2 spawnPos = GetFrontSpawnPos();

        // 카메라 오른쪽 경계보다 안쪽이면 생성 X
        float cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x;
        if (spawnPos.x <= cameraRightEdge) return;

        // 장애물과 겹치는지 확인
        bool overlaps = Physics2D.OverlapCircle(spawnPos, overlapCheckRadius, obstacleLayer);
        if (overlaps) return;

        // 아이템 생성
        int index = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[index], spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// (한종민)플레이어 앞쪽 지정 거리와 수직 범위 내에서,
    /// 화면 밖으로 벗어난 위치를 계산하여 반환합니다.
    /// </summary>
    /// <returns>플레이어 기준 앞쪽 랜덤 스폰 위치 (Vector2)</returns>
    Vector2 GetFrontSpawnPos()
    {
        float x = player.position.x + forwardDistance;
        float y = player.position.y + Random.Range(-verticalRange, verticalRange);
        return new Vector2(x, y);
    }
}
