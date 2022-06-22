using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentBatteryController : MonoBehaviour {
    public float minimumLevel;
    public float batteryLevel;

    private List<Transform> statusIndicators = new List<Transform>();
    private DateTime lastUpdate = DateTime.Now;
    private AudioSource audioSource;
    private Transform activator;
    private Transform sourceBattery;
    private BatteryState state;
    private BatteryState lastState;
    private bool stateChanged;

    public enum BatteryState {
        Charging,
        Transmiting,
        Default
    }

    void Start() {
        foreach (Transform child in transform) {
            statusIndicators.Add(child);
        }
        lastState = state = BatteryState.Default;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Update() {
        if ((DateTime.Now - lastUpdate).TotalMilliseconds < 400)
            return;
        lastUpdate = DateTime.Now;

        TestCollision();
        UpdateState();
        ChangeIndicatorLevel();
        UpdateBlinking();
        PlaySound();
    }

    private void TestCollision() {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1f)) {
            if (hitInfo.transform.CompareTag("Battery")) {
                sourceBattery = hitInfo.transform;
                state = BatteryState.Charging;
            } else if (hitInfo.transform.CompareTag("Activator")) {
                activator = hitInfo.transform;
                state = BatteryState.Transmiting;
            } else {
                state = BatteryState.Default;
            }
        }
    }

    private void UpdateBlinking() {
        if (batteryLevel <= minimumLevel) {
            foreach (var indicator in statusIndicators) {
                var animation = indicator.GetComponent<Animation>();
                if (animation.isPlaying)
                    break;
                animation.Play();
                animation.Rewind();
                animation.Play();
                animation.wrapMode = WrapMode.Loop;
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
                audioSource.SetClipFromResource("charging");
                audioSource.Play();
                break;

            case BatteryState.Transmiting:
                if (!stateChanged || audioSource.isPlaying)
                    return;
                audioSource.loop = false;
                audioSource.SetClipFromResource("activator");
                audioSource.Play();
                break;
        }
    }

    private void UpdateState() {
        switch (state) {
            case BatteryState.Charging:
                if (batteryLevel > 1.0f) {
                    state = BatteryState.Default;
                } else {
                    batteryLevel += 0.04f;
                }
                sourceBattery.GetComponent<BatteryController>().state = BatteryController.BatteryState.Emitting;
                break;

            case BatteryState.Transmiting:
                if (batteryLevel <= minimumLevel) {
                    batteryLevel = minimumLevel;
                    state = BatteryState.Default;
                } else {
                    if (activator == null)
                        return;
                    batteryLevel -= 0.02f;
                    LevelState.Instance.SetActivatorState(activator, true);
                }
                break;

            case BatteryState.Default:
                if (sourceBattery != null) {
                    sourceBattery.GetComponent<BatteryController>().state = BatteryController.BatteryState.StopEvents;
                    sourceBattery = null;
                }
                if(activator != null) {
                    LevelState.Instance.SetActivatorState(activator, false);
                    activator = null;
                }
                if (batteryLevel <= minimumLevel)
                    batteryLevel = minimumLevel;
                else
                    batteryLevel -= 0.01f;
                break;
        }
        if (lastState != state)
            stateChanged = true;
        else
            stateChanged = false;
        lastState = state;
    }

    private void ChangeIndicatorLevel() {
        foreach (var indicator in statusIndicators) {
            indicator.localScale = new Vector3(indicator.localScale.x, batteryLevel * 0.1f, indicator.localScale.z);
        }
    }
}