using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box;
    Rigidbody2D rigidbody;


    public float Speed = 1.0f;
    public float JumpPower = 10f;
    public bool Dead = false;
    public bool Jumps = false;
    public bool Slides = false;
    public bool Wakes = false;
    public bool grounds = true;
    public bool Drop = false;
    float deathCool = 0f;
    private GameOver gameOverManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        gameOverManager = FindObjectOfType<GameOver>();
        rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            if (deathCool >= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetInteger("doDead", 0);
                }
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space) && grounds)
            {
                Jumps = true;
                grounds = false;
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift) && grounds)
            {
                animator.SetTrigger("Slide");
                box.size = new Vector2(1f, 0.5f);
                box.offset = new Vector2(0f, -0.25f);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && grounds)
            {
                animator.SetTrigger("Wakes");
                box.size = new Vector2(1f, 1f);
                box.offset = new Vector2(0, 0f);
            }
        }
    }
    public void FixedUpdate()
    {

        rigidbody.velocity = new Vector2(Speed, rigidbody.velocity.y);

        if (Dead)
            return;


        if (Jumps)
        {
            rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            Jumps = false;
        }


        Debug.Log($"슬라이드 {rigidbody.velocity.y}");

        if (rigidbody.velocity.y < 0f) // 최대 높이 값   
        {
            if (!Drop)
            {
                Drop = true;
                animator.SetBool("Drop",Drop);
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounds = true;
            Debug.Log("Ground");
            animator.SetTrigger("Run");

            if (Drop)
            {
                Drop = false;
                animator.SetBool("Drop", Drop);
                
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            animator.SetTrigger("Dead");
            Dead = true;
            deathCool = 1f;

            /// <summary>
            /// (한종민) 블록과 충돌 시 1초 후 게임오버 연출을 시작함
            /// </summary>
            if (gameOverManager != null)
                StartCoroutine(DelayedGameOver(1.0f)); // 여기서 1초 후 암전 시작
        }
    }

    /// <summary>
    /// (한종민) 일정 시간 후 GameOverManager.StartGameOver() 호출
    /// </summary>
    IEnumerator DelayedGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverManager.StartGameOver();
    }
}
