using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    private void Update() {
        transform.position = player.position + new Vector3(0, 2, -10);
    }
}
