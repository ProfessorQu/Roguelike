using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerDash dash;

    Image health;

    public Color blinkColor;
    Color defaultColor;

    Color[] colors = new Color[2];

    public float blinkInterval = 0.2f;
    private float blinkTimer;

    private int colorIndex = 0;

    private void Start() {
        blinkTimer = blinkInterval;

        health = GetComponent<Image>();
        defaultColor = health.color;

        colors[0] = defaultColor;
        colors[1] = blinkColor;
    }

    private void Update() {
        if (dash.dashesLeft < 1f) {
            if (blinkTimer <= 0f) {
                colorIndex++;
                colorIndex %= colors.Length;

                blinkTimer = blinkInterval;
            }

            health.color = colors[colorIndex];

            blinkTimer -= Time.deltaTime;
        }
        else {
            health.color = defaultColor;
        }
    }
}
