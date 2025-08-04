using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupportMessageSpawner : MonoBehaviour
{
    public GameObject messagePrefab;
    [SerializeField] private RectTransform canvasTransform;

    private List<RectTransform> spawnedRects = new List<RectTransform>();

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
        floating.Setup(line.text, fontAsset, sizeFont, alpha);

        // 강제 렌더링 갱신 → 텍스트 실제 크기 측정
        Canvas.ForceUpdateCanvases();
        tmp.ForceMeshUpdate();

        Vector2 preferred = tmp.GetPreferredValues(line.text, 1000, 0);
        Vector2 size = new Vector2(preferred.x, preferred.y);
        float width = canvasTransform.rect.width * 0.8f;
        float height = canvasTransform.rect.height * 0.8f;
        // 충돌 회피 위치 계산
        Vector2 pos = Vector2.zero;
        bool found = false;
        const int maxAttempts = 20;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 candidate = new Vector2(
            Random.Range(-width / 2f, width / 2f),
            Random.Range(-height / 2f, height / 2f)
            );
            Rect myRect = new Rect(candidate - size / 2f, size);

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

            float fallbackWidth = canvasTransform.rect.width * 0.8f;
            float fallbackHeight = canvasTransform.rect.height * 0.8f;

            float x = Random.Range(-fallbackWidth / 2f, fallbackWidth / 2f);
            float y = Random.Range(-fallbackHeight / 2f, fallbackHeight / 2f);
            pos = new Vector2(x, y);

            go.transform.SetAsLastSibling();

            // ✅ X축까지 밀어내기 추가
            pos += new Vector2(
                Random.Range(-60f, 60f),
                Random.Range(30f, 80f)
            );
        }

        // 🔐 타이핑 전에 자리 고정 (중요!)
        rt.anchoredPosition = pos;
        floating.SetOrigin(pos);
        spawnedRects.Add(rt);
    }
}