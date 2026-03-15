using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 10;

    FighterController owner;

    void Start()
    {
        owner = GetComponentInParent<FighterController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        FighterController enemy = other.GetComponent<FighterController>();

        if (enemy != null && enemy != owner)
        {
            FighterHealth hp = enemy.GetComponent<FighterHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}