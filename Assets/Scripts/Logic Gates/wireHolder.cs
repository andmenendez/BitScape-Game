using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireHolder : MonoBehaviour
{
    public Transform holdPosition;
    public float snapDistance = 1.0f;

    public GameObject[] toggleObjects;
    public Material defaultMaterial;
    public Material onMaterial;
    public Material offMaterial;

    private GameObject[] allWires;
    private GameObject wireHeld;
    private bool isHolding = false;

    public bool power = false;
    public bool state = false;

    public bool is_fixed_wire = false;
    public bool inverse = false;
    public GameObject fixedEnd;
    public bool isGenerator = false;

    public bool is_connected
    {
        get
        {
            if (isHolding) 
            {
                return wireHeld.GetComponent<wireEndNode>().get_wire_controller.is_connected;
            }
            else
            {
                return false;
            }
        }
    }

    public bool is_powered
    {
        get
        {
                return power;
        }
    }

    public bool wire_is_powered
    {
        get
        {
            return wireHeld.GetComponent<wireEndNode>().get_wire_controller.is_powered;
        }
    }

    public bool get_state
    {
        get
        {
            return state;
        }
    }

    public void disconnect()
    {
        if (isHolding)
        {
            wireHeld.GetComponent<wireEndNode>().disconnect();
        }
        materialNull();
    }
    public void connect()
    {
        if (isHolding)
        {
            wireHeld.GetComponent<wireEndNode>().connect_to(this);
        }
        materialNull();
    }
    public void set_state(bool newState)
    {
        if (isHolding)
        {
            wireHeld.GetComponent<wireEndNode>().set_state(newState);
            state = newState;
        }
    }
    public void set_power(bool newPower)
    {
        power = newPower;
        updateMaterial();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (is_fixed_wire)
        {
            if (inverse)
            {
                state = false;
                materialON();
            }
            wireHeld = fixedEnd;
            wireHeld.GetComponent<wireEndNode>().is_fixed = true;
            wireHeld.GetComponent<wireEndNode>().set_state(state);

            wireHeld.transform.position = holdPosition.position;
            wireHeld.GetComponent<Rigidbody>().isKinematic = true;
            wireHeld.GetComponent<wireEndNode>().connect_to(this);
            isHolding = true;
            state = wireHeld.GetComponent<wireEndNode>().get_state;
        }
        else
        {
            allWires = GameObject.FindGameObjectsWithTag("Wire");
            materialNull();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_fixed_wire)
        {
            if (!isHolding)
            {
                for (int i = 0; i < allWires.Length; i++)
                {
                    if (Vector3.Distance(allWires[i].transform.position, holdPosition.position) < snapDistance)
                    {
                        wireHeld = allWires[i];
                        wireHeld.GetComponent<Rigidbody>().isKinematic = true;
                        wireHeld.transform.position = holdPosition.position;
                        state = wireHeld.GetComponent<wireEndNode>().get_state;
                        wireHeld.GetComponent<wireEndNode>().connect_to(this);
                        isHolding = true;
                    }
                }
            } 
            else
            {
                if (Vector3.Distance(wireHeld.transform.position, holdPosition.position) > snapDistance)
                {
                    wireHeld.GetComponent<Rigidbody>().isKinematic = false;
                    wireHeld.GetComponent<wireEndNode>().detach();
                    isHolding = false;
                    state = false;
                }
            }
        } else
        {
            if (isGenerator)
                wireHeld.GetComponent<wireEndNode>().set_state(state);
        }
        updateMaterial();
    }
    void FixedUpdate()
    {
        if (!is_fixed_wire)
        {
            if (isHolding)
            {
                state = wireHeld.GetComponent<wireEndNode>().get_state;
            }
        }
    }

    void updateMaterial()
    {
        if (power || is_fixed_wire)
        {
            if (state)
                materialON();
            else
                materialOFF();
        }
        else
        {
            if (isHolding)
            {
                // If the wire it is touching is powered
                if (wire_is_powered)
                {
                    if (state)
                        materialON();
                    else
                        materialOFF();
                }
            }
            else
            {
                materialNull();
            }
                
        }
    }
    void materialON()
    {  
        if (!inverse)
        {
            if (toggleObjects.Length > 0)
            {
                for (int i = 0; i < toggleObjects.Length; i++)
                {
                    toggleObjects[i].GetComponent<Renderer>().material = onMaterial;
                }
            }
        }
        else
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
    void materialOFF()
    {
        if (!inverse)
        {
            if (toggleObjects.Length > 0)
            {
                for (int i = 0; i < toggleObjects.Length; i++)
                {
                    toggleObjects[i].GetComponent<Renderer>().material = offMaterial;
                }
            }
        }
        else
        {
            if (toggleObjects.Length > 0)
            {
                for (int i = 0; i < toggleObjects.Length; i++)
                {
                    toggleObjects[i].GetComponent<Renderer>().material = onMaterial;
                }
            }
        }
    }
    void materialNull()
    {
        if (toggleObjects.Length > 0)
        {
            for (int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].GetComponent<Renderer>().material = defaultMaterial;
            }
        }
    }
}