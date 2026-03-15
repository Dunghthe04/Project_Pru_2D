using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    private FighterController fightController;

    private float attackTimer = 0f;
    public float attackDelay = 0.5f;

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
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", Mathf.Abs(fightController.MoveInput));
        anim.SetBool("Jump", !fightController.IsGrounded);
    }

    void HandleAttack()
    {
        if (fightController.AttackPressed)
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
}