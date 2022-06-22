using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireEndNode : MonoBehaviour
{
    //
    public bool isConnected = false;
    public float forceMagnitude = 500f;
    public bool is_fixed;
    //
    public GameObject groundObj = null;
    public float groundLevel = 0.0f;
    //
    private wireHolder connector = null;
    private Rigidbody rb;
    //
    private AudioClip open;
    private AudioClip close;
    private AudioSource audioSource;


    void Start()
    {
        if (!is_fixed)
        {
            open = (AudioClip)Resources.Load($"Sounds/WIRE_CONNECT");
            close = (AudioClip)Resources.Load($"Sounds/WIRE_DISCONNECT");
            audioSource = GetComponent<AudioSource>();
        }

        rb = GetComponent<Rigidbody>();
        if (groundObj != null)
        {
            groundLevel = groundObj.transform.position.y;
        }
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        //Debug.Log("HERE");
        if (!is_fixed)
        {
            if (rb.isKinematic || !rb.useGravity || isConnected)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                this.detach();
            }
        }
    }

    public wireController get_wire_controller
    {
        get
        {
            return transform.parent.gameObject.GetComponent<wireController>();
        }
    }

    public bool is_connected
    {
        get
        {
            return isConnected;
        }
    }
    public bool is_powered
    {
        get
        {
            if (connector != null)
                return connector.is_powered;
            else
                return false;
        }
    }
    public void connect_to(wireHolder holder)
    {
        connector = holder;
        isConnected = true;
        transform.parent.gameObject.GetComponent<wireController>().updateStateMaterials();
        if (!is_fixed)
            audioSource.PlayOneShot(open, 0.75F);
    }

    public void disconnect()
    {
        isConnected = false;
        transform.parent.gameObject.GetComponent<wireController>().updateStateMaterials();
        if (!is_fixed)
            audioSource.PlayOneShot(close, 0.75F);
    }
    public void detach()
    {
        connector = null;
        isConnected = false;
        transform.parent.gameObject.GetComponent<wireController>().updateStateMaterials();
        if (!is_fixed)
            audioSource.PlayOneShot(close, 0.75F);
    }

    public void relapse(Transform objTransform)
    {
        if (!is_fixed)
        {
            rb.isKinematic = false;
            //rb.freezeRotation = false;
            rb.useGravity = true;
            rb.AddForce((objTransform.position - transform.position) * forceMagnitude);
        }
    }

    public bool get_state
    {
        get
        {
            return transform.parent.gameObject.GetComponent<wireController>().get_state;
        }
    }
    public void set_state(bool newState)
    {
        transform.parent.gameObject.GetComponent<wireController>().set_state(newState);
    }

    void Update()
    {
        if (transform.position.y < groundLevel)
        {
            //Debug.Log("WIRE RESET ABOVE FLOOR");
            transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
        }
    }
}
