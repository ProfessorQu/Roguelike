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
    public ParticleSystem afterImage;
    private ParticleSystem.EmissionModule afterImageEmission;
    
    private Animator anim;

    Player player;

    [Header("Variables")]
    public float dashVelocity;
    private Vector2 dashDir;

    public float dashTime;

    public float dashLeft = 1;
    public float rechargeRate = 0.1f;

    public float dashDestroyRadius = 2;

    [Header("Camera Shake")]
    public float shakeIntensity = 5;
    public float shakeDuration = 0.1f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        player = GetComponent<Player>();

        anim = GetComponent<Animator>();

        afterImageEmission = afterImage.emission;
        afterImageEmission.enabled = false;
        
        dashTimer.SetTime(dashTime);
    }

    private void Update() {
        if (dashTimer.running) {
            rb.velocity = dashDir * dashVelocity;

            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, dashDestroyRadius, player.whatIsGround);
            foreach (var coll in collisions) {
                if (coll.CompareTag("Destructible")) {
                    Destroy(coll.gameObject);
                }
            }

            afterImageEmission.enabled = true;
        }
        if (dashTimer.Tick()) {
            ResetState();
        }

        if (dashLeft < 1f) {
            dashLeft += Time.deltaTime * rechargeRate;
        }

        if (dashLeft > 1f) {
            dashLeft = 1f;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.started && !dashTimer.running && dashLeft == 1f) {
            dashDir = player.direction;
            if (player.NoDirection()) {
                float horizontal = transform.localScale.x;
                if (horizontal < 0f) {
                    horizontal = -1f;
                }
                else {
                    horizontal = 1f;
                }
                dashDir = new Vector2(horizontal, 0);
            }

            player.FreezeControl(true);

            rb.gravityScale = 0;

            dashTimer.Start();
            isDashing = true;

            dashLeft = 0f;

            CameraShake.Instance.Shake(shakeIntensity, shakeDuration);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isDashing) {
            if (other.collider.CompareTag("Enemy")) {
                other.gameObject.GetComponent<EnemyAI>().Kill();
            }
            else if (other.collider.CompareTag("Destructible")) {
                Destroy(other.gameObject);
            }
            else {
                ResetState();
            }
        }
    }

    private void ResetState() {
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 1;

        dashTimer.Reset();
        
        player.FreezeControl(false);
        afterImageEmission.enabled = false;
        isDashing = false;
    }

    private void OnDrawGizmos() {
        if (dashDestroyGizmos) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, dashDestroyRadius);
        }
    }
}
