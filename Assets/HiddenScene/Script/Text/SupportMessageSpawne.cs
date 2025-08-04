using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupportMessageSpawner : MonoBehaviour
{
    public GameObject messagePrefab;
    [SerializeField] private RectTransform canvasTransform;
    private List<RectTransform> spawnedRects = new List<RectTransform>();
    private List<Vector2> usedPositions = new List<Vector2>();

    public void SpawnMessage(PromptLine line)
    {
        GameObject go = Instantiate(messagePrefab, canvasTransform);
        FloatingMessageUI floating = go.GetComponent<FloatingMessageUI>();
        RectTransform rt = go.GetComponent<RectTransform>();

        // 위치 설정
        Vector2 pos = GetNonOverlappingPosition();
        rt.anchoredPosition = pos;

        // 폰트 로드
        TMP_FontAsset fontAsset = null;
        if (!string.IsNullOrEmpty(line.font))
            fontAsset = Resources.Load<TMP_FontAsset>(line.font);

        // 알파, 폰트 크기 기본값 보정
        float size = line.fontSize;
        float alpha = (line.alpha >= 0f && line.alpha <= 1f) ? line.alpha : 1f;

        floating.Setup(line.text, fontAsset, size, alpha);
        spawnedRects.Add(rt);
    }

    private Vector2 GetNonOverlappingPosition()
    {
        const int maxAttempts = 100;
        const float minDistance = 150f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // ✅ 넓은 영역에 퍼지도록 범위 설정
            float x = Random.Range(-800f, 800f);
            float y = Random.Range(-400f, 400f);
            Vector2 candidate = new Vector2(x, y);

            bool overlaps = false;
            foreach (Vector2 pos in usedPositions)
            {
                if (Vector2.Distance(candidate, pos) < minDistance)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                usedPositions.Add(candidate);
                return candidate;
            }
        }

        return new Vector2(Random.Range(-500f, 500f), Random.Range(-250f, 250f));
    }
}