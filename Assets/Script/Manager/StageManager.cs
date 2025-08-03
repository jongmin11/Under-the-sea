using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public GameObject[] stageObjects; // 스테이지 오브젝트 배열
    public SpriteRenderer fadeOverlay; // 검정 배경 Sprite

    public Obstacle obstacleGenerator;

    private int currentStageIndex = 0;
    private int loopCount = 0;
    private bool isTransitioning = false;

    public void IncrementLoopCount()
    {

        loopCount++;

        if (loopCount >= 10 && !isTransitioning)
        {
            int nextStageIndex = currentStageIndex + 1;
           
            if (nextStageIndex < stageObjects.Length)
            {
                obstacleGenerator.SetStage(nextStageIndex);
                StartCoroutine(TransitionToNextStage(nextStageIndex));
            }

            loopCount = 0;
        }
    }
    IEnumerator TransitionToNextStage(int nextStageIndex)
    {
        isTransitioning = true;

        // 1. 플레이어 Rigidbody 잠시 정지 (선택)
        Rigidbody2D playerRb = GameObject.FindWithTag("Player")?.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.isKinematic = true; // 중력 영향 끊기
        }

        // 현재 카메라 위치
        Vector3 cameraPos = Camera.main.transform.position;

        // 현재 Stage 비활성화
        stageObjects[currentStageIndex].SetActive(false);

        // 다음 스테이지 위치 재배치
        stageObjects[nextStageIndex].transform.position = new Vector3(cameraPos.x, 0f, 0f);

        // 다음 Stage 활성화
        stageObjects[nextStageIndex].SetActive(true);
        currentStageIndex = nextStageIndex;

        // Rigidbody 다시 활성화
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }

        isTransitioning = false;
        yield break;
    }

    IEnumerator FadeBackground(float fromAlpha, float toAlpha, float duration)
    {
        float time = 0f;

        // 시작 컬러를 정확히 설정 (RGB 유지, 알파만 조정)
        Color startColor = fadeOverlay.color;
        startColor.a = fromAlpha;
        fadeOverlay.color = startColor;

        // 목표 컬러 설정
        Color endColor = fadeOverlay.color;
        endColor.a = toAlpha;
        

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            // 선형 보간으로 부드러운 알파 변화
            Color currentColor = Color.Lerp(startColor, endColor, t);
            fadeOverlay.color = currentColor;

            yield return null;
        }

        // 마지막 단계에서 목표 알파를 정확히 설정
        fadeOverlay.color = endColor;
        
    }
   
}
