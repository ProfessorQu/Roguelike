using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer render;

    // GROUND CHECK
    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    // GENERAL
    [Header("General")]
    public float speed = 8f;
    public float maxFallSpeed = -10f;
    
    private Vector2 direction;
    private float horizontal;
    private bool facingRight = true;

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

    // JUMP
    [Header("Jump")]
    public float rememberGroundedTime = 0.1f;
    private float groundedTimer;

    public float rememberJumpInputTime = 0.1f;
    private float rememberJumpTimer;

    [Space]
    public float jumpForce = 16f;
    public float jumpEndedFallSpeed = 0.5f;

    private bool jumpCanceled;

    private void Start() {
        // Get Components
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();

        // Setup trail
        trail.emitting = false;
        trail.time = dashTime;
    }

    private void Update() {
        // Dashing
        if (dashTimer > 0f) {
            rb.velocity = direction * dashVelocity;

            // Break blocks
            Collider2D[] blocks = Physics2D.OverlapCircleAll(transform.position, dashDestroyRadius, whatIsGround);
            foreach (var block in blocks) {
                if (block.CompareTag("Destructible")) {
                    Destroy(block.gameObject);
                }
            }

            trail.emitting = true;
        }
        // Normal movement
        else {
            float horizontalVel = horizontal * speed;
            rb.velocity = new Vector2(horizontalVel, rb.velocity.y);

            trail.emitting = false;
        }

        // Reset velocity when dash stopped
        if (!dashFinished && dashTimer <= 0f) {
            rb.velocity = new Vector2(0, 0);
            dashFinished = true;
        }

        // Flip character
        if (!facingRight && horizontal > 0f) {
            Flip();
        }
        else if (facingRight && horizontal < 0f) {
            Flip();
        }

        // Set grounded timer
        if (IsGrounded()) {
            groundedTimer = rememberGroundedTime;
        }

        // Jump
        if (groundedTimer > 0f && rememberJumpTimer > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            groundedTimer = 0f;
            rememberJumpTimer = 0f;
        }
        // Shorter jump
        else if (jumpCanceled) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedFallSpeed);
            jumpCanceled = false;
        }

        // Clamp the fall speed
        if (rb.velocity.y < maxFallSpeed && dashTimer <= 0f) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }

        // Recharge dashes
        if (dashesLeft < 3f) {
            dashesLeft += Time.deltaTime * (1 / dashRechargeTime);
        }
        // Clamp dashes
        else if (dashesLeft > 3f) {
            dashesLeft = 3f;
        }

        // Decrease timers
        groundedTimer -= Time.deltaTime;
        rememberJumpTimer -= Time.deltaTime;

        dashTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        dashTimer = 0f;
    }

    private bool IsGrounded() {
        // Check if the player is grounded
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void Flip() {
        // Flip the player
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void Jump(InputAction.CallbackContext context) {
        // Get jump input
        if (context.performed) {
            rememberJumpTimer = rememberJumpInputTime;
        }

        if (context.canceled && rb.velocity.y > 0f) {
            jumpCanceled = true;
        }
    }

    public void Move(InputAction.CallbackContext context) {
        // Get move input
        Vector2 newDir = context.ReadValue<Vector2>();
        horizontal = newDir.x;

        if (dashTimer <= 0f) {
            direction = newDir.normalized;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        // Get dash input
        if (context.started && dashesLeft > 1f) {
            // If no direction set, use left/right
            if (Mathf.Abs(direction.x) < 0.1f && Mathf.Abs(direction.y) < 0.1f) {
                direction = new Vector2(transform.localScale.x, 0);
                direction.Normalize();
            }

            // Update variables
            dashTimer = dashTime;
            dashFinished = false;
            
            dashesLeft--;

            // Shake camera
            CameraShake.Instance.Shake(dashShakeIntensity, dashShakeDuration);
        }
    }
}
