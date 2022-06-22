using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpenOnWire : MonoBehaviour
{
    private Animator anim;
    public Collider wall;
    public wireHolder wireInput;
    private AudioClip open;
    private AudioSource audioSource;
    private bool isOpenned = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        open = (AudioClip)Resources.Load($"Sounds/space_door_open");
    }

    // Update is called once per frame
    void Update()
    {
        if (wireInput.get_state)
        {
            if (!isOpenned)
            {
                audioSource.PlayOneShot(open, 0.5F);
            }
                
            isOpenned = true;
            anim.SetBool("character_nearby", wireInput.get_state);
            wall.enabled = false;
        }
    }
}
