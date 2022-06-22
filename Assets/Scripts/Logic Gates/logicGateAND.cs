using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicGateAND : MonoBehaviour
{
    public wireHolder gateA;
    public wireHolder gateB;
    public wireHolder gateOut;

    public Material onMaterial;
    public Material offMaterial;

    public GameObject[] toggleObjects;

    public wireController outputWire;

    private bool state = false;

    private AudioClip open;
    private AudioClip close;
    private AudioSource audioSource;

    // Update is called once per frame
    void Start()
    {
        open = (AudioClip)Resources.Load($"Sounds/AND_CONFIRMATION");
        close = (AudioClip)Resources.Load($"Sounds/AND_DISCONNECT");
        audioSource = GetComponent<AudioSource>();
        materialOFF();
        gateOut.set_power(true);
    }
    void Update()
    {
        if (gateA.get_state && gateB.get_state)
        {
            if (!state)
            {
                audioSource.PlayOneShot(open, 1.0F);
                materialON();
                setOutput(true);
            }
            state = true;
        }
        else
        {
            if (state)
            {
                audioSource.PlayOneShot(close, 1.0F);
                materialOFF();
                setOutput(false);
            }
            state = false;
        }
    }

    void setOutput(bool newState)
    {
        outputWire.set_state(newState);
    }

    void materialON()
    {
        for (int i = 0; i < toggleObjects.Length; i++)
        {
            toggleObjects[i].GetComponent<Renderer>().material = onMaterial;
        }
    }
    void materialOFF()
    {
        for (int i = 0; i < toggleObjects.Length; i++)
        {
            toggleObjects[i].GetComponent<Renderer>().material = offMaterial;
        }
    }
}



