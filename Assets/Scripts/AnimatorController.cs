using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    private FighterController fightController;

    private float attackTimer = 0f;
    public float attackDelay = 0.5f;

    private float skillTimer = 0f;
    public float skillDelay = 1.0f;

    [Header("Skill Projectile")]
    public GameObject skillPrefab;
    public Transform skillSpawnPoint;

    public GameObject attackPoint;

    void Start()
    {
        anim = GetComponent<Animator>();
        fightController = GetComponent<FighterController>();
    }

    void Update()
    {
        UpdateAnimations();
        HandleAttack();
        HandleSkill();
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", Mathf.Abs(fightController.MoveInput));
        anim.SetBool("Jump", !fightController.IsGrounded);
        anim.SetBool("Block", fightController.IsBlocking);
    }
    public void PlayHurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void PlayDeath()
    {
        anim.SetTrigger("Death");
    }
    void HandleAttack()
    {
        if (fightController.AttackPressed && !anim.GetBool("Attack") && !anim.GetBool("Skill") && !fightController.IsBlocking)
        {
            attackTimer = attackDelay;
            anim.SetBool("Attack", true);
            attackPoint.GetComponent<Collider2D>().enabled = true;
        }

        if (anim.GetBool("Attack"))
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                anim.SetBool("Attack", false);
                attackPoint.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void HandleSkill()
    {
        // Điều kiện để được dùng Skill: Bấm nút, chưa đang Attack/Skill và không bị Block
        if (fightController.SkillPressed && !anim.GetBool("Skill") && !anim.GetBool("Attack") && !fightController.IsBlocking)
        {
            skillTimer = skillDelay;
            anim.SetBool("Skill", true);

            // Bắn ra quả cầu năng lượng của nhân vật
            if (skillPrefab != null && skillSpawnPoint != null)
            {
                GameObject proj = Instantiate(skillPrefab, skillSpawnPoint.position, Quaternion.identity);

                // Xác định hướng bay và lật hình ảnh dựa vào nhân vật
                float casterDirection = fightController.transform.localScale.x > 0 ? 1f : -1f;

                Vector3 projScale = proj.transform.localScale;
                projScale.x = Mathf.Abs(projScale.x) * casterDirection;
                proj.transform.localScale = projScale;

                SkillProjectile logic = proj.GetComponent<SkillProjectile>();
                if (logic != null)
                {
                    logic.ownerPlayerID = fightController.playerID;
                    logic.direction = casterDirection;
                }
            }
        }

        if (anim.GetBool("Skill"))
        {
            skillTimer -= Time.deltaTime;

            if (skillTimer <= 0)
            {
                anim.SetBool("Skill", false);
            }
        }
    }
}