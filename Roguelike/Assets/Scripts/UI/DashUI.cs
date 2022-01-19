using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public PlayerDash player;
    public Image[] dashes;

    [Space]
    public Color dashRecharging;
    public Color dashReady;

    private void Update() {
        // Update all dash images
        for (int i = 0; i < dashes.Length; i++) {
            float fillAmount = Mathf.Clamp(player.dashesLeft - i, 0, 1);

            // Set color
            if (fillAmount == 1f) {
                dashes[i].color = dashReady;
            }
            else {
                dashes[i].color = dashRecharging;
            }

            // Set fill amount
            dashes[i].fillAmount = fillAmount;
        }
    }
}
