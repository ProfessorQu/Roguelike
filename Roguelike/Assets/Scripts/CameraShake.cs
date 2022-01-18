using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeStartTimer;
    private float shakeTimer;

    private float startIntensity;

    private void Awake() {
        // Get variables/Singleton
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void Shake(float intensity, float duration) {
        // Shake the camera
        startIntensity = intensity;
        shakeStartTimer = duration;

        noise.m_AmplitudeGain = intensity;
        shakeTimer = duration;
    }

    public void ChangeFrequency(float freq) {
        noise.m_FrequencyGain = freq;
    }

    private void Update() {
        if (shakeTimer <= 0f && noise.m_AmplitudeGain > 0f) {
            noise.m_AmplitudeGain = Mathf.Lerp(
                startIntensity, 0f,
                1 - (shakeTimer / shakeStartTimer)
            );
        }
        else {
            shakeTimer -= Time.deltaTime;
        }
    }
}
