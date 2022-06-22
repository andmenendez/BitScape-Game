using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicGateXOR_hardcoded : MonoBehaviour
{
    public wireHolder gateA;
    public wireHolder gateB;

    public Material onMaterial;
    public Material offMaterial;

    public GameObject[] toggleObjects;

    public wireHolder[] wireHoldersA;
    public wireHolder[] wireHoldersB;
    public wireEndNode[] wireNodes;

    public wireController outputWire;
    public bool debug = false;
    private bool state = false;

    private AudioClip open;
    private AudioClip close;
    private AudioSource audioSource;

    private bool A;
    private bool B;
    private bool C;
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

        if (debug)
            Debug.Log("A: " + A + "/" + wireHoldersA.Length
                        + " B: " + B + "/" + wireHoldersB.Length
                           + " C: " + C + "/" + wireNodes.Length
                           + "|||>" + gateA.get_state + " | " + gateB.get_state );

        if (gateA.is_connected && gateB.is_connected)
        {
            A = true;
            B = true;
            C = true;

            if (wireHoldersA.Length > 0) {
                A = false;
                for (int i = 0; i < wireHoldersA.Length; i++)
                { A = A || wireHoldersA[i].is_connected; }
            }

            if (wireHoldersB.Length > 0)
            {
                B = false;
                for (int i = 0; i < wireHoldersB.Length; i++)
                { B = B || wireHoldersB[i].is_connected; }
            }

            if (wireNodes.Length > 0)
            { 
                for (int i = 0; i < wireNodes.Length; i++)
                { C = C && wireNodes[i].is_connected; }
            }

            if ( A && B && C && ((!gateA.get_state && gateB.get_state) || (gateA.get_state && !gateB.get_state)))
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



