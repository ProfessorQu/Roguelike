using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerDash dash;

    Image health;

    public Color[] blinkColors;
    public Color defaultColor;

    public float blinkInterval = 0.2f;
    private float blinkTimer;

    private int colorIndex = 0;

    private void Start() {
        blinkTimer = blinkInterval;

        health = GetComponent<Image>();
    }

    private void Update() {
        if (dash.dashesLeft < 1f) {
            if (blinkTimer <= 0f) {
                colorIndex++;
                colorIndex %= blinkColors.Length;

                blinkTimer = blinkInterval;
            }

            health.color = blinkColors[colorIndex];

            blinkTimer -= Time.deltaTime;
        }
        else {
            health.color = defaultColor;
        }
    }
}
