using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    private FighterController fightController;
    private FighterHealth fighterHealth;

    private float attackTimer = 0f;
    public float attackDelay = 0.5f;

    private float skillTimer = 0f;
    public float skillDelay = 0f;

    private float ultimateTimer = 0f;
    public float ultimateDelay = 0f;

    [Header("Skill Projectile")]
    public GameObject skillPrefab;
    public Transform skillSpawnPoint;
    public bool spawnSkillOnEnemy = false;

    [Header("Ultimate Projectile")]
    public GameObject ultimatePrefab;
    public Transform ultimateSpawnPoint;
    public bool spawnUltimateOnEnemy = false;

    [Header("Melee Hitboxes")]
    public GameObject attackPoint;
    public GameObject skillPoint;
    public GameObject ultimatePoint;

    [Header("Melee Damage Options")]
    public int attackDamage = 5;
    public int skillMeleeDamage = 15;
    public int ultimateMeleeDamage = 35;

    void Start()
    {
        anim = GetComponent<Animator>();
        fightController = GetComponent<FighterController>();
        fighterHealth = GetComponent<FighterHealth>();
    }

    void Update()
    {
        UpdateAnimations();
        HandleAttack();
        HandleSkill();
        HandleUltimate();
    }

    void UpdateAnimations()
    {
        float currentSpeed = Mathf.Abs(fightController.MoveInput);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool inActionState = stateInfo.IsName("Attack") || stateInfo.IsName("Skill") || stateInfo.IsName("Ultimate") || stateInfo.IsTag("Attack") || stateInfo.IsTag("Skill") || stateInfo.IsTag("Ultimate");

        if (!inActionState && anim.GetCurrentAnimatorClipInfoCount(0) > 0)
        {
            string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower();
            if (clipName.Contains("attack") || clipName.Contains("skill") || clipName.Contains("ultimate"))
            {
                inActionState = true;
            }
        }

        if (anim.GetBool("Attack") || anim.GetBool("Skill") || anim.GetBool("Ultimate") || inActionState)
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
        if (fightController.AttackPressed && !anim.GetBool("Attack") && !anim.GetBool("Skill") && !anim.GetBool("Ultimate") && !fightController.IsBlocking && !inAttackState)
        {
            attackTimer = attackDelay;
            anim.SetBool("Attack", true);

            // Gắn số liệu damage cho hitbox trước khi đánh
            AttackHitbox ah = attackPoint.GetComponent<AttackHitbox>();
            if (ah != null) ah.damage = attackDamage;

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
        if (fightController.SkillPressed && !anim.GetBool("Skill") && !anim.GetBool("Attack") && !anim.GetBool("Ultimate") && !fightController.IsBlocking)
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
            else
            {
                GameObject hitbox = skillPoint != null ? skillPoint : attackPoint;
                if (hitbox != null)
                {
                    // Đẩy số liệu damage cao hơn vào hitbox khi dùng skill cận chiến
                    AttackHitbox ah = hitbox.GetComponent<AttackHitbox>();
                    if (ah != null) ah.damage = skillMeleeDamage;

                    hitbox.GetComponent<Collider2D>().enabled = true;
                }
            }
        }

        if (anim.GetBool("Skill"))
        {
            skillTimer -= Time.deltaTime;

            if (skillTimer <= 0)
            {
                anim.SetBool("Skill", false);
                GameObject hitbox = skillPoint != null ? skillPoint : attackPoint;
                if (hitbox != null)
                {
                    hitbox.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }

    void HandleUltimate()
    {
        // Điều kiện sử dụng Ultimate: Bấm nút, không bị khóa, và đủ mana
        if (fightController.UltimatePressed && !anim.GetBool("Ultimate") && !anim.GetBool("Skill") && !anim.GetBool("Attack") && !fightController.IsBlocking)
        {
            if (fighterHealth != null && fighterHealth.currentMana >= 80)
            {
                fighterHealth.currentMana -= 80; // Trừ mana
                ultimateTimer = ultimateDelay;
                anim.SetBool("Ultimate", true);

                if (ultimatePrefab != null)
                {
                    if (spawnUltimateOnEnemy)
                    {
                        StartCoroutine(SpawnUltimateWithDelay(0.3f));
                    }
                    else
                    {
                        SpawnUltimateProjectile();
                    }
                }
                else
                {
                    GameObject hitbox = ultimatePoint != null ? ultimatePoint : (skillPoint != null ? skillPoint : attackPoint);
                    if (hitbox != null)
                    {
                        AttackHitbox ah = hitbox.GetComponent<AttackHitbox>();
                        if (ah != null) ah.damage = ultimateMeleeDamage;

                        hitbox.GetComponent<Collider2D>().enabled = true;
                    }
                }
            }
        }

        if (anim.GetBool("Ultimate"))
        {
            ultimateTimer -= Time.deltaTime;

            if (ultimateTimer <= 0)
            {
                anim.SetBool("Ultimate", false);
                GameObject hitbox = ultimatePoint != null ? ultimatePoint : (skillPoint != null ? skillPoint : attackPoint);
                if (hitbox != null)
                {
                    hitbox.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }

    private IEnumerator SpawnUltimateWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnUltimateProjectile();
    }

    private void SpawnUltimateProjectile()
    {
        Vector3 spawnPos = transform.position;

        if (ultimateSpawnPoint != null)
        {
            spawnPos = ultimateSpawnPoint.position;
        }

        if (spawnUltimateOnEnemy)
        {
            FighterController[] allFighters = FindObjectsOfType<FighterController>();
            foreach (FighterController fighter in allFighters)
            {
                if (fighter.playerID != fightController.playerID)
                {
                    spawnPos = fighter.transform.position;
                    spawnPos.y -= 0.2f;
                    break;
                }
            }
        }

        GameObject proj = Instantiate(ultimatePrefab, spawnPos, Quaternion.identity);

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