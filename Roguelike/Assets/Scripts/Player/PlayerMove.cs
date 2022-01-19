using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    // GENERAL
    [Header("General")]
    public PlayerDash dash;

    private bool freezeControl;

    public float speed = 8f;
    public float maxFallSpeed = -10f;
    
    [HideInInspector] public Vector2 direction;
    private Vector2 frozenDirection;

    private float horizontal;
    private bool facingRight = true;

    [HideInInspector] public float fallSpeed;

    private void Start() {
        // Get Components
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<PlayerDash>();
    }

    private void Update() {
        // Normal movement
        if (!freezeControl) {
            float horizontalVel = horizontal * speed;
            if (fallSpeed == float.MaxValue) {
                fallSpeed = rb.velocity.y;
            }

            rb.velocity = new Vector2(horizontalVel, fallSpeed);
        }

        // Flip character
        if (!facingRight && horizontal > 0f) {
            Flip();
        }
        else if (facingRight && horizontal < 0f) {
            Flip();
        }

        // Clamp the fall speed
        if (rb.velocity.y < maxFallSpeed && !dash.isDashing) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }

        fallSpeed = float.MaxValue;
    }

    public void FreezeControl(bool freeze) {
        freezeControl = freeze;
    }

    private void Flip() {
        // Flip the player
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context) {
        // Get move input
        Vector2 newDir = context.ReadValue<Vector2>();
        horizontal = newDir.x;

        if (!freezeControl) {
            direction = newDir.normalized;
        }
    }
}
