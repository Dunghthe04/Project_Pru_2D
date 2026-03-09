using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public int playerID = 1;

    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    float baseScale;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = Mathf.Abs(transform.localScale.x);
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float move = 0;

        if (playerID == 1)
        {
            if (Input.GetKey(KeyCode.A)) move = -1;
            if (Input.GetKey(KeyCode.D)) move = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1;
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move > 0)
            transform.localScale = new Vector3(baseScale, baseScale, 1);

        if (move < 0)
            transform.localScale = new Vector3(-baseScale, baseScale, 1);
    }

    void Jump()
    {
        if (!isGrounded) return;

        if (playerID == 1 && Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}