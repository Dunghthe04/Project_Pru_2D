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
    private Animator anim;
    public bool IsGrounded { get; private set; }
    public float MoveInput { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool IsBlocking { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseScale = Mathf.Abs(transform.localScale.x);
    }

    void Update()
    {
        // Khóa mọi hành động nếu đang dùng skill
        if (anim != null && anim.GetBool("Skill"))
        {
            MoveInput = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
            IsBlocking = false;
            AttackPressed = false;
            SkillPressed = false;
            return;
        }

        ReadBlockInput();
        Move();
        Jump();
        ReadAttackInput();
        ReadSkillInput();
    }

    void Move()
    {
        float move = 0;

        if (!IsBlocking)
        {
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
        }

        MoveInput = move;
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move > 0)
            transform.localScale = new Vector3(baseScale, baseScale, 1);

        if (move < 0)
            transform.localScale = new Vector3(-baseScale, baseScale, 1);
    }

    void Jump()
    {
        if (!IsGrounded || IsBlocking) return;

        if (playerID == 1 && Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void ReadBlockInput()
    {
        IsBlocking = playerID == 1
            ? Input.GetKey(KeyCode.S)
            : Input.GetKey(KeyCode.DownArrow);
    }

    void ReadAttackInput()
    {
        AttackPressed = playerID == 1
            ? Input.GetKeyDown(KeyCode.J)
            : Input.GetKeyDown(KeyCode.Alpha1);
    }

    void ReadSkillInput()
    {
        SkillPressed = playerID == 1
            ? Input.GetKeyDown(KeyCode.U)
            : Input.GetKeyDown(KeyCode.Alpha4);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}