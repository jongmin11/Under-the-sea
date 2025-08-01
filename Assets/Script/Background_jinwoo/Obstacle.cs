using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [System.Serializable]
    public class StageObstacleSet // Inspector에서 장애물 프리팹들을 Stage 단위로 묶어 등록할 수 있도록 도와주는 클래스
    {
        public GameObject[] topObstaclePrefabs;
        public GameObject[] bottomObstaclePrefabs;
    }

    public List<StageObstacleSet> stageObstacleSets;
    public GameObject[] stageObjects; // 각 스테이지의 루트 오브젝트들

    public Transform obstacleSpawnRoot;

    public int obstacleCount = 10;           // 생성할 총 장애물 수
    public float minIntervalX = 6f;          // 최소 X 간격
    public float maxIntervalX = 12f;         // 최대 X 간격

    public float topY = 0f;                // Top 장애물 Y 위치
    public float bottomY = -2.5f;            // Bottom 장애물 Y 위치
    public float minTopBottomDistance = 5f;  // Top과 Bottom X 간격 최소

    public float reuseOffsetX = 60f; // 재사용 시 얼마나 앞쪽에 다시 배치할지

    private Vector3 lastTopXPos = Vector3.zero;
    private Vector3 lastBottomXPos = Vector3.zero;

    private int bottomStreak = 0;
    private int currentStageIndex = 0;
    void Start()
    {
        GenerateObstacles();
    }
    public void GenerateObstacles()
    {
        Vector3 currentPosition = obstacleSpawnRoot != null ? obstacleSpawnRoot.position : transform.position;

        StageObstacleSet currentSet = stageObstacleSets[currentStageIndex];

        int bottomCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            float spawnX = currentPosition.x + Random.Range(minIntervalX, maxIntervalX);

            bool canSpawnBottom = bottomStreak < 2 && Mathf.Abs(spawnX - lastTopXPos.x) >= minTopBottomDistance;

            bool forceBottom = (bottomCount < 2 && i >= obstacleCount - 2); // 끝에 최소 3개는 보장

            bool spawnBottom = (Random.value < 0.5f && canSpawnBottom) || forceBottom;

            if (spawnBottom)
            {
                GameObject bottom = Instantiate(GetRandom(currentSet.bottomObstaclePrefabs), new Vector3(spawnX, bottomY, 0), Quaternion.identity);
                bottom.tag = "Obstacle";
                lastBottomXPos = bottom.transform.position;
                bottomStreak++;
                bottomCount++;
            }
            else
            {
                int topCount = Random.Range(1, 4);
                float topStartX = spawnX + 1f; // Bottom과 간섭 피하기 위해 X축 약간 이동
                for (int t = 0; t < topCount; t++)
                {
                    float topX = topStartX + t * 1.5f;
                    GameObject top = Instantiate(GetRandom(currentSet.topObstaclePrefabs), new Vector3(topX, topY, 0), Quaternion.identity);
                    top.tag = "Obstacle";
                    lastTopXPos = top.transform.position;
                }
                bottomStreak = 0;
            }

            currentPosition.x = spawnX + Random.Range(4f, 6f); // 장애물 간격 증가
        }
    }
    
  
    GameObject GetRandom(GameObject[] prefabs)
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("프리팹 배열이 비어있습니다.");
            return null;
        }

        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    public void ReuseObstacle(GameObject obstacle)
    {
        Vector3 newPosition = obstacle.transform.position;
        newPosition.x += reuseOffsetX;
        obstacle.transform.position = newPosition;
    }

    public void SetStage(int index)
    {
        if (index >= 0 && index < stageObstacleSets.Count)
        {
            currentStageIndex = index;

            // 기존 장애물 제거
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Destroy(obj);
            }

            lastTopXPos = Vector3.zero;
            lastBottomXPos = Vector3.zero;
            bottomStreak = 0;

                // obstacleSpawnRoot를 stageObjects[index]의 위치로 설정
            if (stageObjects != null && index < stageObjects.Length && stageObjects[index] != null)
            {
                obstacleSpawnRoot = stageObjects[index].transform;
                Vector3 cameraPos = Camera.main.transform.position;
                obstacleSpawnRoot.position = new Vector3(cameraPos.x, obstacleSpawnRoot.position.y, 0f);
            }
           
            else
            {
                // stageObjects가 없으면 기본 transform 사용 (안전장치)
                obstacleSpawnRoot = this.transform;
                Debug.LogWarning("stageObjects 배열이 비어있거나 올바르게 할당되지 않았습니다.");
            }

            GenerateObstacles();
        }
        else
        {
            Debug.LogWarning("잘못된 스테이지 인덱스입니다.");
        }
    }
    
}
