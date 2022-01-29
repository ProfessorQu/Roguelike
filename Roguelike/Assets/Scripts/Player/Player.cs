using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public PlayerJump jump;
    [HideInInspector] public PlayerDash dash;
    [HideInInspector] public PlayerExit exit;

    [Header("Gizmos")]
    public bool groundCheckGizmos;
    public bool wallCheckGizmos;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask whatIsGround;
    

    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallCheckSize;
    
    [Header("Dust Particles")]
    public GameObject dustParticles;

    public Vector2 direction;

    public bool freezeControl;

    private void Start() {
        move = GetComponent<PlayerMove>();
        jump = GetComponent<PlayerJump>();
        dash = GetComponent<PlayerDash>();
        exit = GetComponent<PlayerExit>();
    }

    private void Update() {
        direction = move.direction;
    }

    public bool NotMoving() {
        return Mathf.Abs(move.direction.x) < 0.1f;
    }

    public bool NoDirection() {
        return Mathf.Abs(move.direction.x) < 0.1f && Mathf.Abs(move.direction.y) < 0.1f;
    }

    public void SpawnDust(Collider2D ground) {
        Vector2 dustPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        GameObject dust = Instantiate(dustParticles, dustPos, Quaternion.identity);

        ParticleSystem particles = dust.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        
        main.startColor = ground.GetComponentInChildren<SpriteRenderer>().color;
        particles.Play();
    }
    
    public void FreezeControl(bool freeze) {
        freezeControl = freeze;
    }

    public Collider2D IsTouchingWall() {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, whatIsGround);
    }

    public Collider2D IsGrounded() {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, whatIsGround);
    }
    
    private void OnDrawGizmos() {
        if (groundCheckGizmos) {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        }

        if (wallCheckGizmos) {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(wallCheck.position, wallCheckSize);
        }
    }
}
