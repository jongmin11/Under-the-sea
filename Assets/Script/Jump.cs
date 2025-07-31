using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody;

    public float JumpPower = 0.1f;
    public bool Dead = false;
    public bool Jumps = false;
    public bool grounds = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            Debug.Log("ав╬З╬Н©Д");
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space) && grounds)
            {
                Jumps = true;
                animator.SetInteger("isJump", 1); 
                grounds = false;
            }
        }
    }
    public void FixedUpdate()
    {
        if (Dead)
            return;

        

        if (Jumps)
        {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = JumpPower;
            rigidbody.velocity = velocity;
            Jumps = false;
            animator.SetInteger("isDrop", 1);
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounds = true; 
            animator.SetInteger("isJump", 0); 
            animator.SetInteger("isDrop", 0); 
        }
    }
}
