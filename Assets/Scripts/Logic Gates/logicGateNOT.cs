using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicGateNOT : MonoBehaviour
{
    public wireHolder gateIN;
    public wireHolder gateOUT;

    public bool power = false;
    public bool a;
    public bool b;  

    public GameObject[] toggleObjects;
    public Material onMaterial;
    public Material offMaterial;

    // Update is called once per frame
    void Update()
    {
        if (gateIN.is_connected && gateIN.wire_is_powered)
        {
            materialOn();
            power = true;
            gateOUT.set_power(true);
            gateOUT.set_state(!gateIN.get_state);

        } 
        else 
        {
            materialOff();
            if (power)
            {
                power = false;
                gateOUT.set_power(false);
                gateOUT.set_state(!gateIN.get_state);
            }
        }
    }
    void materialOn()
    {
        if (toggleObjects.Length > 0)
        {
            for (int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].GetComponent<Renderer>().material = onMaterial;
            }
        }
    }
    void materialOff()
    {
        if (toggleObjects.Length > 0)
        {
            for (int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].GetComponent<Renderer>().material = offMaterial;
            }
        }
    }

}
