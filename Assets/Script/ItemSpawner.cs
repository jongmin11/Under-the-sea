using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (������)���� �ð����� �������� �����ϸ�, ��ֹ��� ��ġ�� �ʵ��� �浹 �˻縦 �����մϴ�.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("������ ������ ���")]
    public GameObject[] itemPrefabs;

    [Header("���� ���� ����")]
    public Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 3f);

    [Header("���� Ÿ�̹�")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("�浹 �˻�")]
    [Tooltip("��ֹ� ���̾�")]
    public LayerMask obstacleLayer;

    [Tooltip("��ħ �˻� ������")]
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

        // ��ġ ���� ����
        Vector2 position = GetRandomPosition();

        // �浹 �˻�
        bool overlaps = Physics2D.OverlapCircle(position, overlapCheckRadius, obstacleLayer);
        if (overlaps) return; // ��ֹ��� ��ġ�� ���� ���

        // ������ ���� ���� ����
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
