using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    
    public float grabDistance = 10.0f;
    public float maxHoldingDistance = 6.0f;
    public float minHoldingDistance = 2.5f;
    public float dropDistance = 3;
    //public float grabWidth = 1.0f;
    public float objectYLimit = 2.25f;

    public List<string> pickupTags;
    public GameObject debugTargetObject;
    public float forceMagnitude = 500;

    private Camera cam;
    private bool isHoldingSomething = false;

    private GameObject objHeld;
    private Rigidbody objBody;
    private float objectDistance;
    private Vector3 holdingPosition;
    private float boundedY;
    private Vector3 defaultPosition;
    private int interactableLayer;

    public Animator anim;


    private AudioClip open;
    private AudioClip close;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        open = (AudioClip)Resources.Load($"Sounds/cast");
        close = (AudioClip)Resources.Load($"Sounds/decast");
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        interactableLayer = LayerMask.GetMask("Interactable");
    }
    void DropItem()
    {
        audioSource.PlayOneShot(close, 1.0F);
        objBody.useGravity = true;
        objBody.freezeRotation = false;
        //objBody.isKinematic = false;
        isHoldingSomething = false;
    }
    void UpdateObjectPosition()
    {
        ////////////////////////////////////////////////////
        holdingPosition = cam.transform.position + cam.transform.forward * objectDistance;
        float boundedY = holdingPosition.y;
        if (boundedY < transform.position.y- objectYLimit)
        {
            boundedY = transform.position.y- objectYLimit;
        } else if (boundedY > transform.position.y + objectYLimit)
        {
            boundedY = transform.position.y + objectYLimit;
        }
        holdingPosition = new Vector3(holdingPosition.x, boundedY, holdingPosition.z);

        if (debugTargetObject != null)
            debugTargetObject.transform.position = holdingPosition;
        
        if (isHoldingSomething)
        {
            ///////////////////////////////////////////////////
            Vector3 forceVector = (holdingPosition - objHeld.transform.position);
            if (forceVector.y < 0)
            {
                forceVector = new Vector3(forceVector.x, 0f, forceVector.z);
            }
            objHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objHeld.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            objHeld.GetComponent<Rigidbody>().AddForce(forceVector.normalized * forceMagnitude * Mathf.Pow(forceVector.magnitude,2));
            ///////////////////////////////////////////////////
        }
    }
    void PickupItem()
    {
        RaycastHit hit;
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, -3f));

        //if (Physics.Raycast(ray, out hit, grabDistance, interactableLayer))
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, grabDistance, interactableLayer))
        {
            Debug.Log(hit.collider.tag);
            if (pickupTags.Contains(hit.collider.tag))
            {
                objectDistance = Vector3.Distance(hit.collider.transform.position, cam.transform.position);
                // Hold object in front so that it doesn't interfere with character
                if (objectDistance > maxHoldingDistance)
                    objectDistance = maxHoldingDistance;
                
                if (objectDistance < minHoldingDistance)
                    objectDistance = minHoldingDistance;
                
                objHeld = hit.collider.transform.gameObject;
                objBody = objHeld.GetComponent<Rigidbody>();
                //objBody.freezeRotation = true;
                //objBody.useGravity = false;
                isHoldingSomething = true;
                //objBody.isKinematic = true;
                audioSource.PlayOneShot(open, 1.0F);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (isHoldingSomething)
        {
            if (!anim.GetBool("isCarryingBox"))
                anim.SetBool("isCarryingBox", true);
        }
        else
        {
            if (anim.GetBool("isCarryingBox"))
                anim.SetBool("isCarryingBox", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isHoldingSomething)
                PickupItem();
            else
                DropItem();
        }            
    }
    void FixedUpdate()
    {
        if (isHoldingSomething)
        {
            UpdateObjectPosition();
            if (Vector3.Distance(holdingPosition, objHeld.transform.position) > dropDistance)
            {
                DropItem();
            }
        }
    }
}

