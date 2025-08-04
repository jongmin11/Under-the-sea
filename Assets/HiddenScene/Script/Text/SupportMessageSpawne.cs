using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupportMessageSpawner : MonoBehaviour
{
    public GameObject messagePrefab;
    public RectTransform spawnArea; // 텍스트 퍼질 캔버스 영역

    private List<RectTransform> spawnedRects = new List<RectTransform>();

    public void SpawnMessage(PromptLine line)
    {
        GameObject go = Instantiate(messagePrefab, spawnArea);
        FloatingMessageUI floating = go.GetComponent<FloatingMessageUI>();
        RectTransform rt = go.GetComponent<RectTransform>();

        // 위치 설정
        Vector2 pos = GetNonOverlappingPosition(rt);
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

    private Vector2 GetNonOverlappingPosition(RectTransform newRect)
    {
        int maxAttempts = 20;
        Vector2 finalPos = Vector2.zero;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPos = GetRandomPositionInArea();
            newRect.anchoredPosition = randomPos;

            Bounds newBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(spawnArea, newRect);

            bool hasOverlap = false;
            foreach (RectTransform other in spawnedRects)
            {
                Bounds otherBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(spawnArea, other);
                if (newBounds.Intersects(otherBounds))
                {
                    hasOverlap = true;
                    Vector3 direction = (other.anchoredPosition - randomPos).normalized;
                    other.anchoredPosition += (Vector2)direction * 30f;
                }
            }

            if (!hasOverlap)
            {
                finalPos = randomPos;
                break;
            }
        }

        return finalPos;
    }

    private Vector2 GetRandomPositionInArea()
    {
        float width = spawnArea.rect.width;
        float height = spawnArea.rect.height;

        float x = Random.Range(-width / 2f, width / 2f);
        float y = Random.Range(-height / 2f, height / 2f);

        return new Vector2(x, y);
    }
}