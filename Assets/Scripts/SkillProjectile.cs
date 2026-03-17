using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifetime = 3f;
    public bool destroyOnHit = true; // Thêm cờ để quyết định có huỷ khi trúng không

    private bool hasHit = false;

    [HideInInspector]
    public int ownerPlayerID;
    [HideInInspector]
    public float direction = 1f;

    void Start()
    {
        // Tự động hủy sau thời gian lifetime để không bay mãi mãi
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Di chuyển quả cầu về phía trước dựa theo hướng của nhân vật
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (hasHit) return;

        FighterHealth health = col.GetComponent<FighterHealth>();
        FighterController controller = col.GetComponent<FighterController>();

        // Nếu chạm vào người chơi và người chơi đó không phải là chủ nhân của quả cầu
        if (health != null && controller != null && controller.playerID != ownerPlayerID)
        {
            health.TakeDamage(damage);
            hasHit = true;

            // Chỉ huỷ Object nếu người dùng đánh dấu (đối với đạn bay bình thường)
            if (destroyOnHit)
            {
                Destroy(gameObject); 
            }
        }
    }
}
