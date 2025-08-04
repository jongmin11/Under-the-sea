using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box;
    Rigidbody2D rigidbody;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip DeadSound;

    public float Speed = 1.0f;
    public float JumpPower = 10f;
    public bool Dead = false;
    public bool Jumps = false;
    public bool Slides = false;
    public bool Wakes = false;
    public bool grounds = true;
    public bool Drop = false;
    float deathCool = 0f;
    public bool godMode = false;
    private GameOver gameOverManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        gameOverManager = FindObjectOfType<GameOver>();
        rigidbody.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
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
                if (DeadSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(DeadSound);
                }
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space) && grounds)
            {
                Jumps = true;
                grounds = false;

                if (jumpSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                    audioSource.PlayOneShot(jumpSound, 0.6f);
                }
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
        if (Time.timeScale == 0f || Dead)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }

        rigidbody.velocity = new Vector2(Speed, rigidbody.velocity.y);

        if (Jumps)
        {
            rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            Jumps = false;
        }

        if (rigidbody.velocity.y < 1f && !Drop)
        {
            Drop = true;
            animator.SetBool("Drop", Drop);
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
            if (godMode)
            {
                Debug.Log("[GodMode] 장애물 충돌 무시됨");
                return;
            }
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
