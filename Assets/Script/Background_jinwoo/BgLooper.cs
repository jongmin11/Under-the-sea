using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 8;
    public StageManager stageManager; // Stage 전환을 요청할 매니저
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered: " + collision.name);

        if (collision.CompareTag("BackGround"))
        {

            float widthOfBgObject = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;
            pos.x += widthOfBgObject * numBgCount;
            collision.transform.position = pos;
            stageManager.IncrementLoopCount(); // 전환 시작 요청
            Debug.Log("LOOP 실행됨: " + collision.name + ", 위치: " + collision.transform.position);
            return;
       
        }
    }
}
