using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public GameObject[] stageObjects; // 스테이지 오브젝트 배열
    public SpriteRenderer fadeOverlay; // 검정 배경 Sprite


    private int currentStageIndex = 0;
    private int loopCount = 0;
    private bool isTransitioning = false;

    public void IncrementLoopCount()
    {
        Debug.Log($"loopCount = {loopCount}, isTransitioning = {isTransitioning}");

        loopCount++;

        if (loopCount >= 10 && !isTransitioning)
        {
           
            if (currentStageIndex < stageObjects.Length - 1)
            {
              
                StartCoroutine(TransitionToNextStage());
            }

            loopCount = 0;
        }
    }
    IEnumerator TransitionToNextStage()
    {
        isTransitioning = true;

        // 1. 현재 카메라 위치 가져오기
        Vector3 cameraPos = Camera.main.transform.position;


        // 페이드 아웃 (자연스럽게)
        yield return StartCoroutine(FadeBackground(1f, 1f, 1.2f)); // 화면 어두워짐 
        // 현재 Stage 비활성화
        stageObjects[currentStageIndex].SetActive(false);
        // 다음 스테이지 인덱스 계산
        currentStageIndex++;
    
        fadeOverlay.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z + 1f); 
        // 2. 다음 스테이지 오브젝트 위치를 카메라 위치로 이동
        stageObjects[currentStageIndex].transform.position = new Vector3(cameraPos.x, 0f, 0f);


        // 3. 다음 스테이지 활성화
        stageObjects[currentStageIndex].SetActive(true);
        // 페이드 인 (자연스럽게)
        yield return StartCoroutine(FadeBackground(1f, 0f, 1.2f)); // 다시 밝아짐 
        isTransitioning = false;
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
