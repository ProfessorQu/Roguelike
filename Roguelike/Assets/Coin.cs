using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform sprite;

    public float upMove = 0.5f;

    private float spriteY = 0f;

    private void Start() {
        sprite = transform.GetChild(0);
    }

    private void Update() {
        Vector2 spritePos = new Vector2(0, spriteY);

        spriteY += Time.deltaTime;
        sprite.transform.position = spritePos;
    }
}
