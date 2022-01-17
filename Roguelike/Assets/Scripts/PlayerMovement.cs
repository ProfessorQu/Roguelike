using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    float horizontal;
    bool facingRight = true;

    public float speed = 8f;
    public float jumpForce = 16f;
    public float jumpEndedFallSpeed = 0.5f;

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
        if (context.performed && IsGrounded()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (context.canceled && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedFallSpeed);
        }
    }
}
