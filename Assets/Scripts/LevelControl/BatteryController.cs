using System;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour {
    public float minimumLevel;
    public float initialCharge = 0.1f;
    public StateVariable<float> batteryLevel;
    public ParticleSystem particles;

    private List<Transform> statusIndicators = new List<Transform>();
    private List<Transform> levelIndicators = new List<Transform>();
    private DateTime lastUpdate = DateTime.Now;
    public AudioClip chargingAudio;
    public AudioClip activateAudio;
    private AudioSource audioSource;
    private GameObject activator;
    public BatteryState state;
    private BatteryState lastState;
    private bool stateChanged;

    public enum BatteryState {
        Charging,
        Transmiting,
        StopEvents,
        Emitting,
        Default
    }


    void Start() {
        batteryLevel = new StateVariable<float>(initialCharge);
        foreach (Transform child in transform) {
            levelIndicators.Add(child.GetChild(0));
            statusIndicators.Add(child);
        }
        lastState = state = BatteryState.StopEvents;
        audioSource = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
        if (particles != null)
            particles.Stop();
    }

    void Update() {
        if ((DateTime.Now - lastUpdate).TotalMilliseconds < 400)
            return;
        lastUpdate = DateTime.Now;

        if (state == BatteryState.Default &&
            !batteryLevel.HasChanged)
            return;
        UpdateState();
        ChangeIndicatorLevel();
        UpdateBlinking();
        PlaySound();
        SetActivatorState();
    }

    private void SetActivatorState() {
        if (activator == null)
            return;
        LevelState.Instance.SetActivatorState(activator.transform, state == BatteryState.Transmiting);
    }

    private void UpdateBlinking() {
        if (batteryLevel.Value <= minimumLevel) {
            foreach (var indicator in statusIndicators) {
                var animation = indicator.GetComponent<Animation>();
                animation.wrapMode = WrapMode.Loop;
                animation.Play();
            }
            
        } else {
            foreach (var indicator in statusIndicators) {
                var animation = indicator.GetComponent<Animation>();
                animation.Rewind();
                animation.Play();
                animation.wrapMode = WrapMode.Loop;
                animation.Sample();
                animation.Stop();
            }
        }
    }

    private void PlaySound() {
        switch (state) {
            case BatteryState.Default:
                if (audioSource.clip is null)
                    return;
                audioSource.Stop();
                audioSource.clip = null;
                break;

            case BatteryState.Charging:
                if (audioSource.isPlaying)
                    return;
                audioSource.loop = true;
                audioSource.clip = chargingAudio;
                audioSource.Play();
                break;

            case BatteryState.Transmiting:
                if (!stateChanged || 
                    audioSource.isPlaying)
                    return;
                audioSource.loop = false;
                audioSource.clip = activateAudio;
                audioSource.Play();
                break;
        }
    }

    private void UpdateState() {
        switch (state) {
            case BatteryState.Charging:
                if (batteryLevel.Value > 1.0f) {
                    state = BatteryState.Default;
                } else {
                    batteryLevel.Value += 0.04f;
                }
                break;

            case BatteryState.Transmiting:
                if (batteryLevel.Value <= minimumLevel) {
                    batteryLevel.SetValue(minimumLevel, false);
                    state = BatteryState.Default;
                } else {
                    batteryLevel.SetValue(batteryLevel.Value - 0.02f);
                }
                break;

            case BatteryState.StopEvents:
                if(particles != null)
                    particles.Stop();
                state = BatteryState.Default;
                break;
            
            case BatteryState.Emitting:
                if (batteryLevel.Value <= minimumLevel) {
                    particles.Stop();
                    break;
                }
                batteryLevel.SetValue(batteryLevel.Value - 0.04f);
                particles.Play();
                break;
        }
        if (lastState != state)
            stateChanged = true;
        else
            stateChanged = false;
        lastState = state;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Charger")) {
            state = BatteryState.Charging;
        } else if (collision.gameObject.CompareTag("Activator")) {
            state = BatteryState.Transmiting;
            activator = collision.gameObject;
        }
        //Debug.Log($"enter:{collision.gameObject.name} state {state}");
    }

    private void OnCollisionExit(Collision collision) {
        //Debug.Log($"exit:{collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Charger") ||
            collision.gameObject.CompareTag("Activator")) {
            state = BatteryState.StopEvents;
        }
    }

    private void ChangeIndicatorLevel() {
        foreach (var indicator in levelIndicators) {
            indicator.localScale = new Vector3(1f, 1f - batteryLevel.Value, 0.1f);
            indicator.localPosition = new Vector3(0, batteryLevel.Value / 2, -0.6f);
        }
    }
}