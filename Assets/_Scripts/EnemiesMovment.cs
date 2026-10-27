
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool startMovingRight = true;
    [SerializeField] private float turnCooldown = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.5f;

    [Header("Wall Checks")]
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private float wallCheckDistance = 0.2f;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool spriteFacesRight = true;

    [Header("Stomp")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float stompBounceForce = 8f;
    [SerializeField] private float stompTolerance = 0.15f;

    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private int direction;
    private float nextTurnTime;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        direction = startMovingRight ? 1 : -1;
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        if (isDead || rb == null)
            return;

        // If checks are not assigned, continue moving rather than stop.
        if (groundCheck == null ||
            wallCheckLeft == null ||
            wallCheckRight == null)
        {
            Move();
            return;
        }

        bool groundAhead = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        Transform activeWallCheck =
            direction > 0 ? wallCheckRight : wallCheckLeft;

        Vector2 wallDirection =
            direction > 0 ? Vector2.right : Vector2.left;

        bool wallAhead = Physics2D.Raycast(
            activeWallCheck.position,
            wallDirection,
            wallCheckDistance,
            groundLayer
        );

        // Turn only when the cooldown has expired.
        if (Time.time >= nextTurnTime &&
            (wallAhead || !groundAhead))
        {
            direction *= -1;
            nextTurnTime = Time.time + turnCooldown;
            UpdateSpriteDirection();
        }

        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void UpdateSpriteDirection()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX =
                (direction < 0) == spriteFacesRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryStomp(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryStomp(collision);
    }

    private void TryStomp(Collision2D collision)
    {
        if (isDead || !collision.gameObject.CompareTag(playerTag))
            return;

        Rigidbody2D playerRb =
            collision.gameObject.GetComponent<Rigidbody2D>();

        if (playerRb == null)
            playerRb = collision.gameObject
                .GetComponentInParent<Rigidbody2D>();

        Collider2D playerCollider = collision.collider;

        if (playerRb == null ||
            playerCollider == null ||
            enemyCollider == null)
            return;

        float enemyTop = enemyCollider.bounds.max.y;
        float playerBottom = playerCollider.bounds.min.y;

        bool landingOnTop =
            playerRb.linearVelocity.y < 0f &&
            playerBottom >= enemyTop - stompTolerance;

        if (!landingOnTop)
            return;

        isDead = true;

        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            stompBounceForce
        );

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (groundCheck != null)
        {
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position +
                Vector3.down * groundCheckDistance
            );
        }

        Gizmos.color = Color.red;

        if (wallCheckLeft != null)
        {
            Gizmos.DrawLine(
                wallCheckLeft.position,
                wallCheckLeft.position +
                Vector3.left * wallCheckDistance
            );
        }

        if (wallCheckRight != null)
        {
            Gizmos.DrawLine(
                wallCheckRight.position,
                wallCheckRight.position +
                Vector3.right * wallCheckDistance
            );
        }
    }
}
