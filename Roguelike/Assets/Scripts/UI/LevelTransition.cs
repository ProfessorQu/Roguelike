using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public static LevelTransition Instance;

    public Animator transition;

    private void Awake() {
        Destroy(Instance);
        Instance = this;
    }

    public void PlayAnimation() {
        transition.SetTrigger("start");
    }

    public void ResetAnimation() {
        transition.SetTrigger("end");
    }
}
