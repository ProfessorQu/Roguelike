using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    Rigidbody2D rb;

    public bool isDashing;

    private Timer dashTimer = new Timer();
    
    [Header("Gizmos")]
    public bool dashDestroyGizmos;

    [Header("Static Variables")]
    public TrailRenderer trail;
    
    private Animator anim;

    PlayerMove move;
    PlayerJump jump;

    [Header("Variables")]
    public float dashVelocity;
    private Vector2 dashDir;

    public float dashTime;

    public float dashesLeft = 3;
    public float rechargeRate = 0.1f;

    public float dashDestroyRadius = 2;

    [Header("Camera Shake")]
    public float shakeIntensity = 5;
    public float shakeDuration = 0.1f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        move = GetComponent<PlayerMove>();
        jump = GetComponent<PlayerJump>();

        anim = GetComponent<Animator>();

        trail.emitting = false;
        trail.time = dashTime;
        
        dashTimer.SetTime(dashTime);
    }

    private void Update() {
        if (dashTimer.running) {
            rb.velocity = dashDir * dashVelocity;

            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, dashDestroyRadius, jump.whatIsGround);
            foreach (var coll in collisions) {
                if (coll.CompareTag("Destructible")) {
                    Destroy(coll.gameObject);
                }
            }

            trail.emitting = true;
        }
        if (dashTimer.Tick()) {
            ResetState();
        }

        if (dashesLeft < 3f) {
            dashesLeft += Time.deltaTime * rechargeRate;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.started && !dashTimer.running && dashesLeft >= 1f) {
            dashDir = move.direction;
            if (move.NotMoving()) {
                float horizontal = transform.localScale.x;
                if (horizontal < 0f) {
                    horizontal = -1f;
                }
                else {
                    horizontal = 1f;
                }
                dashDir = new Vector2(horizontal, 0);
            }

            move.FreezeControl(true);

            rb.gravityScale = 0;

            dashTimer.Start();
            isDashing = true;

            dashesLeft--;

            CameraShake.Instance.Shake(shakeIntensity, shakeDuration);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isDashing && jump.IsTouchingWall()) {
            ResetState();
        }
    }

    private void ResetState() {
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 1;

        dashTimer.Reset();
        
        move.FreezeControl(false);
        trail.emitting = false;
        isDashing = false;
    }

    private void OnDrawGizmos() {
        if (dashDestroyGizmos) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, dashDestroyRadius);
        }
    }
}
