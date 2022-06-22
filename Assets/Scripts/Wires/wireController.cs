using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireController : MonoBehaviour
{
    public wireEndNode[] endNodes;
    public float wireDisconnectLength = 20f;
    public Material onMaterial;
    public Material offMaterial;
    public Material defaultMaterial;

    private bool alarm = false;
    private bool state = false;


    public void set_state(bool newState)
    {
        state = newState;
        updateStateMaterials();
    }


    public bool get_state
    {
        get
        {
            return state;
        }
    }

    public bool is_powered
    {
        get
        {
            for (int i = 0; i < endNodes.Length; i++)
            {
                if (endNodes[i].is_powered)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool is_connected
    {
        get
        {
            for (int i = 0; i < endNodes.Length; i++)
            {
                if (!endNodes[i].is_connected)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void updateStateMaterials()
    {
        if (endNodes[0].is_powered || endNodes[endNodes.Length - 1].is_powered)
        {
            if (state)
            {
                endNodes[0].GetComponent<Renderer>().material = onMaterial;
                endNodes[endNodes.Length - 1].GetComponent<Renderer>().material = onMaterial;

            } else {
                endNodes[0].GetComponent<Renderer>().material = offMaterial;
                endNodes[endNodes.Length - 1].GetComponent<Renderer>().material = offMaterial;
            }
        } else {
            endNodes[0].GetComponent<Renderer>().material = defaultMaterial;
            endNodes[endNodes.Length - 1].GetComponent<Renderer>().material = defaultMaterial;
        }
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(endNodes[0].transform.position, endNodes[endNodes.Length-1].transform.position) > wireDisconnectLength)
        {
            if (!alarm)
            {
                endNodes[0].GetComponent<Renderer>().material = offMaterial;
                endNodes[endNodes.Length - 1].GetComponent<Renderer>().material = offMaterial;
            }

            Debug.Log(Vector3.Distance(endNodes[0].transform.position, endNodes[endNodes.Length - 1].transform.position));
            
            endNodes[0].relapse(endNodes[endNodes.Length - 1].transform);
            endNodes[endNodes.Length - 1].relapse(endNodes[0].transform);
            alarm = true;
        }
        else
        {
            if (alarm)
            {
                endNodes[0].GetComponent<Renderer>().material = defaultMaterial;
                endNodes[endNodes.Length - 1].GetComponent<Renderer>().material = defaultMaterial;
            }
            alarm = false;
        }
    }
}
