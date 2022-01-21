using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float time;
    public float currentTime;

    public bool running;

    public void SetTime(float time_) {
        time = time_;
        currentTime = time_;
    }

    public void Start() {
        running = true;
    }

    public void Stop() {
        running = false;
    }

    public void Reset() {
        currentTime = time;
        Stop();
    }

    public bool Tick() {
        if (running) {
            currentTime -= Time.deltaTime;

            return currentTime <= 0f;
        }

        return false;
    }
}
