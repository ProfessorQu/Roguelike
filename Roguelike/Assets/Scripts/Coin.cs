using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform sprite;

    public float upLimit = 0.5f;
    public float downLimit = 0.5f;
    [Space]
    public float moveSpeed = 0.2f;
    private float spriteY = 0f;

    private void Start() {
        sprite = transform.GetChild(0);

        spriteY = Random.Range(downLimit, upLimit);
    }

    private void Update() {
        if (sprite.transform.localPosition.y >= upLimit && moveSpeed > 0) {
            moveSpeed *= -1;
        }
        else if (sprite.transform.localPosition.y <= downLimit && moveSpeed < 0) {
            moveSpeed *= -1;
        }

        spriteY += moveSpeed * Time.deltaTime;
        Vector2 spritePos = new Vector2(0, spriteY);
        sprite.transform.localPosition = spritePos;
    }
}
