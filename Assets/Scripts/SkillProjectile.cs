using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifetime = 3f;

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
        FighterHealth health = col.GetComponent<FighterHealth>();
        FighterController controller = col.GetComponent<FighterController>();

        // Nếu chạm vào người chơi và người chơi đó không phải là chủ nhân của quả cầu
        if (health != null && controller != null && controller.playerID != ownerPlayerID)
        {
            health.TakeDamage(damage);
            // Có thể thêm hiệu ứng nổ ở đây
            Destroy(gameObject); 
        }
    }
}
