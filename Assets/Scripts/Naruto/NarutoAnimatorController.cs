using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarutoAnimatorController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public float speed = 5f;
    public float jumpForce = 8f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(move));

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("Jump", true);
        }

        if (Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            anim.SetBool("Jump", false);
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("Attack");
        }
    }
}
