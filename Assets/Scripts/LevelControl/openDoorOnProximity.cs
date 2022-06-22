using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoorOnProximity : MonoBehaviour
{
    public GameObject proximityObject;
    public float proximityThreshold = 5;
    public AudioClip instructions;
    private Animator anim;
    private AudioSource audioSource;
    private AudioClip close;
    private AudioClip open;

    public bool playClip = false;
    private bool played = false;
    private bool toplay = false;

    // Start is called before the first frame update
    void Start()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        open = (AudioClip)Resources.Load($"Sounds/space_door_open");
        close = (AudioClip)Resources.Load($"Sounds/space_door_close");
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(proximityObject.transform.position, transform.position) < proximityThreshold)
        {
            if (!anim.GetBool("character_nearby") && instructions != null)
            {
                audioSource.PlayOneShot(open, 0.5F);
                toplay = true;

                
            }
            anim.SetBool("character_nearby", true);
        }
        else
        {
            if (anim.GetBool("character_nearby") && !playClip)
            {
                audioSource.PlayOneShot(close, 0.5F);
            }
            anim.SetBool("character_nearby", false);
        }

        if (playClip && toplay && !audioSource.isPlaying)
        {
            if (!played)
            {
                audioSource.PlayOneShot(instructions, 0.75F);
                played = true;
            }
        }
    }
}
