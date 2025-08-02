using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 8;
    public StageManager stageManager; // Stage 전환을 요청할 매니저

    public Obstacle obstacle;
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BackGround"))
        {

            float widthOfBgObject = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;
            pos.x += widthOfBgObject * numBgCount;
            collision.transform.position = pos;
            stageManager.IncrementLoopCount(); // 전환 시작 요청
            return;

        }
        if (collision.CompareTag("Obstacle"))
        {
           obstacle.ReuseObstacle(collision.gameObject);
        }
    }
}
