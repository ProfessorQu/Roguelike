using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerDash dash;

    public DashUI dashUI;

    public GameObject damageParticles;

    [Header("Camera Shake")]
    public float intensity = 1f;
    public float duration = 0.2f;

    private void Start() {
        dash = GetComponent<PlayerDash>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Enemy")) {
            if (!dash.isDashing) {
                if (dash.dashLeft >= 1f) {
                    dash.dashLeft = 0;
                    dashUI.Damage();

                    Instantiate(damageParticles, other.contacts[0].point, Quaternion.identity);

                    CameraShake.Instance.Shake(intensity, duration);
                }
                else {
                    Destroy(gameObject);
                }
            }
        }
    }
}
