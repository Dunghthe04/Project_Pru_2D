using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 10;

    private Rigidbody2D rb;

    public void Init(Vector2 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir.normalized * speed;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FighterController enemy = other.GetComponent<FighterController>();

        if (enemy != null)
        {
            FighterHealth hp = enemy.GetComponent<FighterHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // đụng tường thì hủy
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}