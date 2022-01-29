using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : EnemyAI
{
    public float speed = 10;

    public GameObject deathParticles;

    [Header("Front Check")]
    public Transform frontCheck;
    public LayerMask whatIsGround;
    public Vector2 frontCheckSize = new Vector2(0.1f, 0.3f);

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0f, whatIsGround)) {
            Destroy(gameObject);
        }
    }

    private void Update() {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (GetObjectsInFront()) {
            direction *= -1;
        }
        
        // Flip character
        UpdateFlip();
    }
    
    public override void Kill()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private bool GetObjectsInFront() {
        Collider2D coll = Physics2D.OverlapBox(frontCheck.position, frontCheckSize, 0f);
        if (coll) {
            return !coll.CompareTag("Player");
        }

        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(frontCheck.position, frontCheckSize);
    }
}
