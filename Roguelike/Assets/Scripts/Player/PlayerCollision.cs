using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Player player;

    public DashUI dashUI;

    public GameObject damageParticles;
    public GameObject collectCoinParticles;

    [Header("Camera Shake")]
    public float intensity = 1f;
    public float duration = 0.2f;

    [Header("Invulnerability")]
    public float invTime = 0.1f;
    private Timer invTimer = new Timer();

    private bool canTakeDamage = true;

    private void Start() {
        player = GetComponent<Player>();

        invTimer.SetTime(invTime);
        invTimer.Start();
    }

    private void Update() {
        if (invTimer.Tick()) {
            canTakeDamage = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Coin")) {
            AudioManager.Instance.Play("Pickup Coin");
            player.coins++;
            
            Instantiate(collectCoinParticles, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        TakeDamage(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
        TakeDamage(other);
    }

    private void TakeDamage(Collision2D other) {
        if (other.collider.CompareTag("Enemy") && !player.dash.isDashing && canTakeDamage) {
            if (player.dash.dashLeft == 1f) {
                AudioManager.Instance.Play("Damage");

                player.dash.dashLeft = 0;
                dashUI.Damage();

                Instantiate(damageParticles, other.contacts[0].point, Quaternion.identity);

                CameraShake.Instance.Shake(intensity, duration);
            }
            else {
                Destroy(gameObject);
            }

            invTimer.Reset();
            invTimer.Start();

            canTakeDamage = false;
        }
    }
}
