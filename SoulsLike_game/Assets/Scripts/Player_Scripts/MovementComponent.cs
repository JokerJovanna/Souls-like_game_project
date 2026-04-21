using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private DodgeComponent dodge;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dodge = GetComponent<DodgeComponent>();
    }

    void FixedUpdate()
    {
        if (dodge != null && dodge.IsDodging) return;
        
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }
}
