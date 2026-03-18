using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public int playerID = 1;

    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip skillSound;
    [SerializeField] private AudioClip ultimateSound; // ⭐ thêm
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private AudioClip landSound;

    float baseScale;

    private Rigidbody2D rb;
    private Animator anim;

    public bool IsGrounded { get; private set; }
    public float MoveInput { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool UltimatePressed { get; private set; }
    public bool IsBlocking { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseScale = Mathf.Abs(transform.localScale.x);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 🔥 KHÓA KHI ĐANG ĐÁNH / SKILL / ULTIMATE
        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            bool inSkillState = stateInfo.IsName("Skill") || stateInfo.IsTag("Skill");
            bool inUltimateState = stateInfo.IsName("Ultimate") || stateInfo.IsTag("Ultimate");
            bool inAttackState = stateInfo.IsName("Attack") || stateInfo.IsTag("Attack");

            // fallback check bằng tên clip
            if (anim.GetCurrentAnimatorClipInfoCount(0) > 0)
            {
                string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower();

                if (clipName.Contains("skill")) inSkillState = true;
                if (clipName.Contains("ultimate")) inUltimateState = true;
                if (clipName.Contains("attack")) inAttackState = true;
            }

            if (anim.GetBool("Skill") || inSkillState ||
                anim.GetBool("Attack") || inAttackState ||
                anim.GetBool("Ultimate") || inUltimateState)
            {
                MoveInput = 0;
                rb.velocity = new Vector2(0, rb.velocity.y);
                IsBlocking = false;
                AttackPressed = false;
                SkillPressed = false;
                UltimatePressed = false;
                return;
            }
        }

        ReadBlockInput();
        Move();
        Jump();
        ReadAttackInput();
        ReadSkillInput();
        ReadUltimateInput();
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
            PlaySound(jumpSound);
        }

        if (playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            PlaySound(jumpSound);
        }
    }

    void ReadBlockInput()
    {
        bool wasBlocking = IsBlocking;

        IsBlocking = playerID == 1
            ? Input.GetKey(KeyCode.S)
            : Input.GetKey(KeyCode.DownArrow);

        if (!wasBlocking && IsBlocking)
        {
            PlaySound(blockSound);
        }
    }

    void ReadAttackInput()
    {
        AttackPressed = playerID == 1
            ? Input.GetKeyDown(KeyCode.J)
            : Input.GetKeyDown(KeyCode.Alpha1);

        if (AttackPressed)
        {
            PlaySound(attackSound);
        }
    }

    void ReadSkillInput()
    {
        SkillPressed = playerID == 1
            ? Input.GetKeyDown(KeyCode.U)
            : Input.GetKeyDown(KeyCode.Alpha4);

        if (SkillPressed)
        {
            PlaySound(skillSound);
        }
    }

    void ReadUltimateInput()
    {
        UltimatePressed = playerID == 1
            ? Input.GetKeyDown(KeyCode.I)
            : Input.GetKeyDown(KeyCode.Alpha5);

        if (UltimatePressed)
        {
            PlaySound(ultimateSound); // ⭐ ultimate riêng
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            bool wasGrounded = IsGrounded;
            IsGrounded = true;

            if (!wasGrounded)
            {
                PlaySound(landSound);
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // ⭐ OPTIONAL: dùng cho Animation Event (pro)
    public void PlayUltimateSound()
    {
        PlaySound(ultimateSound);
    }
}