using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour
{
    public wireController poweredWire;
    public wireHolder wireholder;
    public bool inverseGenerator;

    // Start is called before the first frame update
    void Start()
    {
        if (inverseGenerator)
        {
            poweredWire.set_state(false);
            wireholder.set_state(false);
            wireholder.inverse = true;
        }
        else
        {
            poweredWire.set_state(true);
            wireholder.set_state(true);
        }
        poweredWire.updateStateMaterials();
    }
}
