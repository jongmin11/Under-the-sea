using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    void Update()
    {
         // 방향키나 A/D 키로 x축 이동
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position += new Vector3(moveX, 0, 0);
    }
}
