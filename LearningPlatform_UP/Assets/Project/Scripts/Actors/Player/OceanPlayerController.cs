using UnityEngine;

public class OceanPlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Joystick joystick;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float drag = 4f;

    [Header("Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -5f);
    public Vector2 maxBounds = new Vector2(10f, 5f);

    private Vector2 inputDirection;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        inputDirection = new Vector2(moveX, moveY);

        // Flip sprite based on horizontal direction
        if (Mathf.Abs(moveX) > 0.1f)
        {
            spriteRenderer.flipX = moveX < 0f;
        }
    }

    void FixedUpdate()
    {
        // Apply movement force
        rb.AddForce(inputDirection * moveSpeed, ForceMode2D.Force);

        // Apply water drag
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, drag * Time.fixedDeltaTime);

        Vector2 pos = rb.position;
        Vector2 vel = rb.linearVelocity;

        // Smoothly zero velocity if hitting X bounds
        if (pos.x <= minBounds.x && vel.x < 0f) vel.x = 0f;
        if (pos.x >= maxBounds.x && vel.x > 0f) vel.x = 0f;

        // Smoothly zero velocity if hitting Y bounds
        if (pos.y <= minBounds.y && vel.y < 0f) vel.y = 0f;
        if (pos.y >= maxBounds.y && vel.y > 0f) vel.y = 0f;

        rb.linearVelocity = vel;
    }
}
