using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public FighterHealth fighter;
    public Image healthBar;

    void Update()
    {
        if (fighter == null || healthBar == null) return;

        float hpPercent = (float)fighter.currentHealth / fighter.maxHealth;
        healthBar.fillAmount = hpPercent;
    }
}