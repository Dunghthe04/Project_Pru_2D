using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int maxMana = 100;
    public int currentMana = 0;
    private float manaTimer = 0f;

    AnimatorController animController;
    FighterController fighterController;

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = 0;
        animController = GetComponent<AnimatorController>();
        fighterController = GetComponent<FighterController>();
    }

    void Update()
    {
        if (currentHealth > 0 && currentMana < maxMana)
        {
            manaTimer += Time.deltaTime;
            if (manaTimer >= 3f)
            {
                currentMana += 10;
                if (currentMana > maxMana)
                {
                    currentMana = maxMana;
                }
                manaTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (fighterController != null && fighterController.IsBlocking)
        {
            damage = 1;
        }

        currentHealth -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHealth);
        if (currentHealth > 0 && damage >= 5)
        {
            animController.PlayHurt();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");

        animController.PlayDeath();

        GetComponent<FighterController>().enabled = false;
    }
}