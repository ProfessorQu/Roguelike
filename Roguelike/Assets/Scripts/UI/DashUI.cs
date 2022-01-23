using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public PlayerDash player;
    public GameObject[] dashes;
    public Image[] dashCharges;

    private Animator[] dashAnims = new Animator[3];

    [Space]
    public Color dashRecharging;
    public Color dashReady;

    private void Start() {
        for (int i = 0; i < dashes.Length; i++) {
            dashAnims[i] = dashes[i].GetComponent<Animator>();
        }
    }

    private void Update() {
        // Update all dash images
        for (int i = 0; i < dashes.Length; i++) {
            float fillAmount = Mathf.Clamp(player.dashesLeft - i, 0, 1);

            // Set color
            if (fillAmount == 1f) {
                dashCharges[i].color = dashReady;
            }
            else {
                dashCharges[i].color = dashRecharging;
            }

            // Set fill amount
            dashCharges[i].fillAmount = fillAmount;
        }
    }

    public void Damage() {
        if (player.dashesLeft >= 2f) {
            dashAnims[2].SetTrigger("damage");
        }
        else if (player.dashesLeft >= 1f) {
            dashAnims[1].SetTrigger("damage");
        }
        else if (player.dashesLeft >= 0f) {
            dashAnims[0].SetTrigger("damage");
        }
    }
}
