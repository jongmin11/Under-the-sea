using UnityEngine;

public class EnemyTextEffectController : MonoBehaviour
{
    public float fadeInTime = 0.5f;
    private float timer = 0f;
    private TextMesh textMesh;
    private Color originalColor;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        originalColor = textMesh.color;
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeInTime);

            // 페이드인
            Color c = textMesh.color;
            c.a = Mathf.Lerp(0, originalColor.a, t);
            textMesh.color = c;

            // 스케일업
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        }
    }

}
