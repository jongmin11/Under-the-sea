using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Animator animator = null;
    Rigidbody2D rigidbody = null;

    public float JumpPower = 20f;
    public bool Dead = false;
    public bool Jumps = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jumps = true;
                animator.SetInteger("Jump", 1);
            }
        }
    }
    public void FixedUpdate()
    {
        if (Dead)
            return;

        Vector3 velocity = rigidbody.velocity;

        if (Jumps)
        {
            velocity.y += JumpPower;
            Jumps = false;
            animator.SetInteger("Drop", 2);
        }
    }
}
