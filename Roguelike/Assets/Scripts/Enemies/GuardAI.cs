using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : EnemyAI
{
    public float speed = 10;
    
    public GameObject deathParticles;

    [Header("Ground in front Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckSize = 0.1f;

    [Header("Front Check")]
    public Transform frontCheck;
    public Vector2 frontCheckSize = new Vector2(0.1f, 0.3f);

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0f, whatIsGround)) {
            Destroy(gameObject);
        }
    }

    private void Update() {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (GetObjectsInFront() | !GroundInFront()) {
            direction *= -1;
        }
        
        // Flip character
        UpdateFlip();
    }

    public override void Kill()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        
        Game_Manager.Instance.kills++;

        Destroy(gameObject);
    }

    private bool GetObjectsInFront() {
        Collider2D coll = Physics2D.OverlapBox(frontCheck.position, frontCheckSize, 0f);
        if (coll) {
            return !coll.CompareTag("Player");
        }

        return false;
    }

    private bool GroundInFront() {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, whatIsGround);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(frontCheck.position, frontCheckSize);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(groundCheck.position, groundCheckSize);
    }
}
