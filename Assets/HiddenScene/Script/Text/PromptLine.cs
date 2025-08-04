[System.Serializable]
public class PromptLine
{
    public string id;
    public string foreignText;  // ⬅️ 외국어 타이핑용
    public string text;         // 한국어
    public string font;     // Resources 폴더 안의 폰트 경로 (ex: "Fonts/MyFont")
    public float fontSize;  // 폰트 크기
    public float alpha;     // 투명도 (0~1)
}