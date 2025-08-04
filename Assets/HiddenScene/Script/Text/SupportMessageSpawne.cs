using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupportMessageSpawner : MonoBehaviour
{
    public GameObject messagePrefab;
    [SerializeField] private RectTransform canvasTransform;

    private List<RectTransform> spawnedRects = new List<RectTransform>();
    float padding = 200f;
    public void SpawnMessage(PromptLine line)
    {
        // 프리팹 인스턴스화
        GameObject go = Instantiate(messagePrefab, canvasTransform);
        RectTransform rt = go.GetComponent<RectTransform>();
        FloatingMessageUI floating = go.GetComponent<FloatingMessageUI>();
        TextMeshProUGUI tmp = go.GetComponentInChildren<TextMeshProUGUI>();

        // 폰트 로드
        TMP_FontAsset fontAsset = null;
        if (!string.IsNullOrEmpty(line.font))
            fontAsset = Resources.Load<TMP_FontAsset>(line.font);

        float sizeFont = line.fontSize > 0 ? line.fontSize : 48f;
        float alpha = (line.alpha >= 0f && line.alpha <= 1f) ? line.alpha : 1f;

        // 텍스트 미리 세팅 (텍스트가 들어가야 크기를 잴 수 있음)
        floating.Setup(line, fontAsset, sizeFont, alpha);

        // 강제 렌더링 갱신 → 텍스트 실제 크기 측정
        Canvas.ForceUpdateCanvases();
        tmp.ForceMeshUpdate();

        Vector2 preferred = tmp.GetPreferredValues(line.text, 1000, 0);
        Vector2 size = new Vector2(preferred.x, preferred.y);
        float width = canvasTransform.rect.width * 17f;
        float height = canvasTransform.rect.height * 10f;
        // 충돌 회피 위치 계산
        Vector2 pos = Vector2.zero;
        bool found = false;
        const int maxAttempts = 20;

        for (int i = 0; i < maxAttempts; i++)
        {
            float skewX = Random.Range(-1f, 1f);
            float skewY = Random.Range(-1f, 1f);
            skewX = Mathf.Pow(skewX, 3) * Mathf.Sign(skewX); // x^3 skew
            skewY = Mathf.Pow(skewY, 3) * Mathf.Sign(skewY);

            Vector2 candidate = new Vector2(
                skewX * width / 2f,
                skewY * height / 2f
            );
            Vector2 paddedSize = size + new Vector2(padding, padding);
            Rect myRect = new Rect(candidate - paddedSize / 2f, paddedSize);

            bool overlap = false;
            foreach (RectTransform other in spawnedRects)
            {
                Rect otherRect = new Rect(
                    other.anchoredPosition - other.sizeDelta / 2f,
                    other.sizeDelta
                );

                if (myRect.Overlaps(otherRect))
                {
                    overlap = true;
                    break;
                }
            }

            if (!overlap)
            {
                pos = candidate;
                found = true;
                break;
            }
        }

        // 회피 실패 → 덮어쓰기 + 살짝 Y 밀기
        if (!found)
        {
            float skewX = Random.Range(-1f, 1f);
            float skewY = Random.Range(-1f, 1f);
            skewX = skewX * skewX * Mathf.Sign(skewX);
            skewY = skewY * skewY * Mathf.Sign(skewY);

            float x = skewX * width / 2f;
            float y = skewY * height / 2f;
            pos = new Vector2(x, y);

            go.transform.SetAsLastSibling();
        }

        // 🔐 타이핑 전에 자리 고정 (중요!)
        rt.anchoredPosition = pos;
        floating.SetOrigin(pos);
        spawnedRects.Add(rt);
    }
}