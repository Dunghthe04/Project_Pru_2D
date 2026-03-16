using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    AnimatorController animController;
    FighterController fighterController;

    void Start()
    {
        currentHealth = maxHealth;
        animController = GetComponent<AnimatorController>();
        fighterController = GetComponent<FighterController>();
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