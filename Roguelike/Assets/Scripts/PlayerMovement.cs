using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    float horizontal;
    bool facingRight = true;

    [Header("Speed")]
    public float speed = 8f;

    public float bombRadius = 10f;

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
    }

    private void Update() {
        float horizontalVel = horizontal * speed;
        rb.velocity = new Vector2(horizontalVel, rb.velocity.y);

        if (!facingRight && horizontal > 0f) {
            Flip();
        }
        else if (facingRight && horizontal < 0f) {
            Flip();
        }

        if (IsGrounded()) {
            groundedTimer = rememberGroundedTime;
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

        groundedTimer -= Time.deltaTime;
        rememberJumpTimer -= Time.deltaTime;
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

    public void Move(InputAction.CallbackContext context) {
        horizontal = context.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            rememberJumpTimer = rememberJumpInputTime;
        }

        if (context.canceled && rb.velocity.y > 0f) {
            jumpCanceled = true;
        }

        // if (context.performed && IsGrounded()) {
        //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        // }

        // if (context.canceled && rb.velocity.y > 0f) {
        //     rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedFallSpeed);
        // }
    }

    public void Bomb(InputAction.CallbackContext context) {
        if (context.started) {
            Collider2D[] blocks = Physics2D.OverlapCircleAll(transform.position, bombRadius);
            foreach (var block in blocks) {
                if (block.CompareTag("Destructible")) {
                    Destroy(block.gameObject);
                }
            }
        }
    }
}
