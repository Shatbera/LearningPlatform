using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody2D rb;

    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float rotationSpeed = 5f;

    private Vector2 velocity = Vector2.zero;
    void Update()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        Vector2 inputDirection = new Vector2(moveX, moveY).normalized;
        float inputStrength = new Vector2(moveX, moveY).magnitude;

        if (inputStrength > 0.1f)
        {
            velocity = Vector2.Lerp(velocity, inputDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = velocity;
    }
}
