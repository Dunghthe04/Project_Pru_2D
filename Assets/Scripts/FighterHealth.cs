using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    AnimatorController animController;

    void Start()
    {
        currentHealth = maxHealth;
        animController = GetComponent<AnimatorController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHealth);
        if (currentHealth > 0)
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