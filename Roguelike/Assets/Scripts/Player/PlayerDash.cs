using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("General")]
    public LayerMask whatIsGround;
    public PlayerMove move;

    private float gravityScale;

    public bool isDashing;

    // DASH
    [Header("Dash")]
    public TrailRenderer trail;

    [Space]
    public float dashVelocity = 12f;

    public float dashTime = 0.2f;
    private float dashTimer;

    [Space]
    public float dashDestroyRadius = 1f;

    private bool dashFinished;

    [Space]
    public float dashesLeft = 3;
    public float dashRechargeTime = 1f;

    [Space]
    public float dashShakeIntensity;
    public float dashShakeDuration = 0.1f;

    private void Start() {
        move = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();

        gravityScale = rb.gravityScale;
        
        // Setup trail
        trail.emitting = false;
        trail.time = dashTime;
    }

    private void Update() {
        // Dashing
        if (dashTimer > 0f) {
            rb.velocity = move.direction * dashVelocity;

            Vector2 dashPosition = new Vector2(transform.position.x + move.direction.x, transform.position.y + move.direction.y);
            // Break blocks
            Collider2D[] blocks = Physics2D.OverlapCircleAll(dashPosition, dashDestroyRadius, whatIsGround);
            foreach (var block in blocks) {
                if (block.CompareTag("Destructible")) {
                    Destroy(block.gameObject);
                }
            }

            trail.emitting = true;
        }
        else {
            isDashing = false;
            trail.emitting = false;
        }
        
        // Reset velocity when dash stopped
        if (!dashFinished && dashTimer <= 0f) {
            rb.velocity = new Vector2(0, 0);

            move.FreezeControl(false);
            rb.gravityScale = gravityScale;

            dashFinished = true;
        }
        
        // Recharge dashes
        if (dashesLeft < 3f) {
            dashesLeft += Time.deltaTime * (1 / dashRechargeTime);
        }
        // Clamp dashes
        else if (dashesLeft > 3f) {
            dashesLeft = 3f;
        }

        dashTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        dashTimer = 0f;
    }

    public void Dash(InputAction.CallbackContext context) {
        // Get dash input
        if (context.started && dashesLeft > 1f) {
            move.FreezeControl(true);
            rb.gravityScale = 0f;

            isDashing = true;

            // If no direction set, use left/right
            if (Mathf.Abs(move.direction.x) < 0.1f && Mathf.Abs(move.direction.y) < 0.1f) {
                move.direction = new Vector2(transform.localScale.x, 0);
                move.direction.Normalize();
            }

            // Update variables
            dashTimer = dashTime;
            dashFinished = false;
            
            dashesLeft--;

            // Shake camera
            CameraShake.Instance.Shake(dashShakeIntensity, dashShakeDuration);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Vector2 dashPosition = new Vector2(transform.position.x + move.direction.x, transform.position.y + move.direction.y);
        Gizmos.DrawWireSphere(dashPosition, dashDestroyRadius);
    }
}
