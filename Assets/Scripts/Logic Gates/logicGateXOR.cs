using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicGateXOR : MonoBehaviour
{
    public wireHolder gateA;
    public wireHolder gateB;

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
        open = (AudioClip)Resources.Load($"Sounds/XOR_CONFIRMATION");
        close = (AudioClip)Resources.Load($"Sounds/XOR_DISCONNECT");
        audioSource = GetComponent<AudioSource>();
        materialOFF();
    }
    void Update()
    {
        if (gateA.is_connected && gateB.is_connected)
        {

            if ((!gateA.get_state && gateB.get_state) || (gateA.get_state && !gateB.get_state))
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
        else
        {
            if (state)
            {
                materialOFF();
                setOutput(false);
                state = false;
            }
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



