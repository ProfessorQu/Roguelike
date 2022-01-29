using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    private TMP_Text coinText;

    private void Start() {
        coinText = GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        coinText.text = Game_Manager.Instance.coins.ToString();
    }
}
