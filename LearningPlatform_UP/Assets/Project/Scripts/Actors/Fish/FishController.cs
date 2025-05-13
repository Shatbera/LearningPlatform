using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Swim Area")]
    public Vector2 minBounds = new Vector2(-10, -5);
    public Vector2 maxBounds = new Vector2(10, 5);

    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float turnSpeed = 2f;
    public float decisionInterval = 3f;

    private Vector2 currentDirection;
    private Vector2 targetDirection;
    private float nextDecisionTime;

    [SerializeField] private Transform flipTransform;
    void Start()
    {
        PickNewDirection();
        currentDirection = targetDirection;
    }

    void Update()
    {
        // Pick a new direction at random intervals
        if (Time.time > nextDecisionTime)
        {
            PickNewDirection();
        }

        // Smooth blend to new direction
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, turnSpeed * Time.deltaTime).normalized;

        // Move fish
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime, Space.World);

        // Flip X if needed
        if (currentDirection.x != 0)
        {
            Vector3 scale = flipTransform.localScale;
            scale.x = Mathf.Sign(currentDirection.x) * Mathf.Abs(scale.x);
            flipTransform.localScale = scale;
        }

        // Clamp inside swim bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        targetDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        nextDecisionTime = Time.time + Random.Range(decisionInterval * 0.8f, decisionInterval * 1.2f);
    }
}
