using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer render;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    float horizontal;
    bool facingRight = true;

    [Header("Speed")]
    private Vector2 direction;

    public float speed = 8f;
    public float maxFallSpeed = -10f;

    [Header("Dash")]
    public float dashVelocity = 12f;
    public float dashTime = 0.2f;
    private float dashTimer;

    private bool canDash;

    [Header("Jump")]
    public float rememberGroundedTime = 0.1f;
    private float groundedTimer;

    public float rememberJumpInputTime = 0.1f;
    private float rememberJumpTimer;

    public float jumpForce = 16f;
    public float jumpEndedFallSpeed = 0.5f;

    private bool jumpCanceled;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (!canDash) {
            render.color = Color.red;
        }
        else {
            render.color = Color.white;
        }

        if (dashTimer > 0f) {
            rb.velocity = direction * dashVelocity;
            render.color = Color.blue;
        }
        else {
            float horizontalVel = horizontal * speed;
            rb.velocity = new Vector2(horizontalVel, rb.velocity.y);
        }

        if (!facingRight && horizontal > 0f) {
            Flip();
        }
        else if (facingRight && horizontal < 0f) {
            Flip();
        }

        if (IsGrounded()) {
            groundedTimer = rememberGroundedTime;
            canDash = true;
        }

        if (groundedTimer > 0f && rememberJumpTimer > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundedTimer = 0f;
            rememberJumpTimer = 0f;
        }
        else if (jumpCanceled) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedFallSpeed);
            jumpCanceled = false;
        }

        if (rb.velocity.y < maxFallSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }

        groundedTimer -= Time.deltaTime;
        rememberJumpTimer -= Time.deltaTime;

        dashTimer -= Time.deltaTime;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            rememberJumpTimer = rememberJumpInputTime;
        }

        if (context.canceled && rb.velocity.y > 0f) {
            jumpCanceled = true;
        }
    }

    public void Move(InputAction.CallbackContext context) {
        Vector2 newDir = context.ReadValue<Vector2>();
        horizontal = newDir.x;

        if (dashTimer <= 0f) {
            direction = newDir.normalized;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.started && canDash) {
            dashTimer = dashTime;
            canDash = false;
        }
    }
}
