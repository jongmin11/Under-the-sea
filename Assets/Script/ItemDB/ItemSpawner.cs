using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (������)���� �ð����� �������� �����ϸ�, ��ֹ��� ��ġ�� �ʵ��� �浹 �˻縦 �����մϴ�.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("������ ������ ���")]
    [Tooltip("������ ������ ������ �迭 (����, ���ǵ� ������ ��)")]
    public GameObject[] itemPrefabs;

    [Header("���� ���� (�÷��̾� ���� ����)")]
    [Tooltip("�÷��̾� ���� ������ �� ���� ������ ���� ��������")]
    public float forwardDistance = 5f;
    [Tooltip("�÷��̾� ���� ���Ʒ��� �󸶳� �۶߸���")]
    public float verticalRange = 2f;

    [Header("���� Ÿ�̹�")]
    [Tooltip("������ ���� ���� (�� ����)")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("�浹 �˻�")]
    [Tooltip("��ġ�� ���� �� �� ��ֹ��� ���̾� ����ũ")]
    public LayerMask obstacleLayer;

    [Tooltip("��ħ üũ�� ���� ���� ������")]
    public float overlapCheckRadius = 0.5f;

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        // �÷��̾� ã��
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("ItemSpawner: 'Player' �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ���� ī�޶� ����
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
    /// (������)�������� ���� ������ ��ġ�� �����ϰ�, ���ǿ� ���� ������ �������� �ʽ��ϴ�.
    /// - ī�޶� ȭ�� ���̸� ���� �� ��
    /// - ��ֹ��� ��ġ�� ���� �� ��
    /// </summary>
    void TrySpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        Vector2 spawnPos = GetFrontSpawnPos();

        // ī�޶� ������ ��躸�� �����̸� ���� X
        float cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x;
        if (spawnPos.x <= cameraRightEdge) return;

        // ��ֹ��� ��ġ���� Ȯ��
        bool overlaps = Physics2D.OverlapCircle(spawnPos, overlapCheckRadius, obstacleLayer);
        if (overlaps) return;

        // ������ ����
        int index = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[index], spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// (������)�÷��̾� ���� ���� �Ÿ��� ���� ���� ������,
    /// ȭ�� ������ ��� ��ġ�� ����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�÷��̾� ���� ���� ���� ���� ��ġ (Vector2)</returns>
    Vector2 GetFrontSpawnPos()
    {
        float x = player.position.x + forwardDistance;
        float y = player.position.y + Random.Range(-verticalRange, verticalRange);
        return new Vector2(x, y);
    }
}
