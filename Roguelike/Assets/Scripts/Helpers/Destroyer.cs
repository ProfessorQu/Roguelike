using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float timeBeforeDestroy;

    private void Start() {
        Destroy(gameObject, timeBeforeDestroy);
    }
}
