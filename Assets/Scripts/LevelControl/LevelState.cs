using System;
using UnityEngine;

public class LevelState : MonoBehaviour {
    public static LevelState Instance { get; private set; }
    public float MouseSpeed { get; set; } = 1.0f;
    public float Volume { get; set; }

    private Material enabledMaterial;
    private Material disabledMaterial;
    private AudioSource instructions;
    private StateVariable<bool> activator1state = new StateVariable<bool>("activator1");
    private StateVariable<bool> activator2state = new StateVariable<bool>("activator2");
    private StateVariable<bool> activator3state = new StateVariable<bool>("activator3");
    private StateVariable<bool> activator4state = new StateVariable<bool>("activator4");
    private StateVariable<bool> door1state = new StateVariable<bool>();
    private StateVariable<bool> door2state = new StateVariable<bool>();
    private Animator door1Anim;
    private Animator door2Anim;
    private int instructionIndex = 1;
    private DateTime lastUpdate;
    private AudioSource audioSource;
    public int currentMaxInstruction = 3;

    private void Awake() {
        Instance = this;
        enabledMaterial = (Material)Resources.Load("Materials/blue emission", typeof(Material));
        disabledMaterial = (Material)Resources.Load("Materials/screen", typeof(Material));
        door1Anim = GameObject.Find("door1")?.GetComponentInChildren<Animator>();
        door2Anim = GameObject.Find("door2")?.GetComponent<Animator>();
        instructions = GameObject.Find("InstructionsPlayer")?.GetComponent<AudioSource>();
        lastUpdate = DateTime.Now;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string clip, GameObject origin, float volume = 1.0f) {
        if (audioSource == null)
            return;
        var clipResource = (AudioClip)Resources.Load($"Sounds/{clip}", typeof(AudioClip));
        audioSource.clip = clipResource;
        audioSource.volume = volume;
        audioSource.Play();
        //Debug.Log($"play sound: {clip}, {origin.name}");
    }

    public void SetActivatorState(Transform activator, bool newState) {
        var activatorName = activator.transform.parent.name;
        var stateChanged = activator1state.SetValue(newState, activatorName) ||
            activator2state.SetValue(newState, activatorName) ||
            activator3state.SetValue(newState, activatorName) ||
            activator4state.SetValue(newState, activatorName);

        if (!stateChanged)
            return;
        activator.GetComponent<MeshRenderer>().material = newState ? enabledMaterial : disabledMaterial;

        var solved1 = activator1state.Value && activator2state.Value;
        door1Anim.SetBool("solved", solved1);
        if (door1state.SetValue(solved1))
            PlaySound(solved1 ? "space_door_open" : "space_door_close", door1Anim.gameObject);

        var solved2 = activator3state.Value && activator4state.Value;
        door2Anim.SetBool("solved", solved2);
        if (door2state.SetValue(solved2))
            PlaySound(solved2 ? "space_door_open" : "space_door_close", door2Anim.gameObject);
    }

    public void Update() {
        if (instructions == null ||
            instructionIndex > currentMaxInstruction)
            return;
        if ((DateTime.Now - lastUpdate).TotalMilliseconds < 5000)
            return;

        lastUpdate = DateTime.Now;

        if (instructions.isPlaying) {
            lastUpdate = DateTime.Now;
            return;
        }

        var clipResource = (AudioClip)Resources.Load($"Sounds/Instruction{instructionIndex}", typeof(AudioClip));
        instructions.clip = clipResource;
        instructions.Play();
        instructionIndex++;
    }
}
