using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    private FighterController fightController;

    private float attackTimer = 0f;
    public float attackDelay = 0.5f;

    private float skillTimer = 0f;
    public float skillDelay = 0f;

    [Header("Skill Projectile")]
    public GameObject skillPrefab;
    public Transform skillSpawnPoint;
    public bool spawnSkillOnEnemy = false;

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
        float currentSpeed = Mathf.Abs(fightController.MoveInput);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool inActionState = stateInfo.IsName("Attack") || stateInfo.IsName("Skill") || stateInfo.IsTag("Attack") || stateInfo.IsTag("Skill");

        if (!inActionState && anim.GetCurrentAnimatorClipInfoCount(0) > 0)
        {
            string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower();
            if (clipName.Contains("attack") || clipName.Contains("skill"))
            {
                inActionState = true;
            }
        }

        if (anim.GetBool("Attack") || anim.GetBool("Skill") || inActionState)
        {
            currentSpeed = 0f;
        }

        anim.SetFloat("Speed", currentSpeed);
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
        // Nhận diện xem Animator có ĐANG chạy hình ảnh Attack hay không
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool inAttackState = stateInfo.IsName("Attack") || stateInfo.IsTag("Attack");

        if (!inAttackState && anim.GetCurrentAnimatorClipInfoCount(0) > 0)
        {
            string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower();
            if (clipName.Contains("attack"))
            {
                inAttackState = true;
            }
        }

        // Bổ sung điều kiện !inAttackState để chặn việc gọi lại đòn đánh khi đòn cũ chưa xong
        if (fightController.AttackPressed && !anim.GetBool("Attack") && !anim.GetBool("Skill") && !fightController.IsBlocking && !inAttackState)
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
            if (skillPrefab != null)
            {
                if (spawnSkillOnEnemy)
                {
                    StartCoroutine(SpawnSkillWithDelay(0.3f));
                }
                else
                {
                    SpawnSkillProjectile();
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

    private IEnumerator SpawnSkillWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnSkillProjectile();
    }

    private void SpawnSkillProjectile()
    {
        Vector3 spawnPos = transform.position;

        // Mặc định spawn ở vị trí định trước
        if (skillSpawnPoint != null)
        {
            spawnPos = skillSpawnPoint.position;
        }

        // Nếu có tuỳ chọn gọi skill ngay dưới chân đối thủ
        if (spawnSkillOnEnemy)
        {
            FighterController[] allFighters = FindObjectsOfType<FighterController>();
            foreach (FighterController fighter in allFighters)
            {
                if (fighter.playerID != fightController.playerID)
                {
                    // Lấy vị trí dưới chân đối thủ (nếu pivot của đối thủ nằm giữa người thì phải trừ đi 1 khoảng y)
                    spawnPos = fighter.transform.position;
                    spawnPos.y -= 0.4f;
                    break;
                }
            }
        }

        GameObject proj = Instantiate(skillPrefab, spawnPos, Quaternion.identity);

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