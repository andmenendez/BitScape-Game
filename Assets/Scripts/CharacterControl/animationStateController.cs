using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animationController;
    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool walkKey = Input.GetKey("w");
        bool runKey = Input.GetKey("left shift");
        bool leftKey = Input.GetKey("a");
        bool rightKey = Input.GetKey("d");

        // Walk Animation
        if (walkKey)
        {
            animationController.SetBool("isWalking", true);
            if (runKey)
            {
                animationController.SetBool("isLeft", false);
                animationController.SetBool("isRight", false);
                animationController.SetBool("isRunning", true);
            }
            else if (leftKey)
            {
                animationController.SetBool("isLeft", true);
                animationController.SetBool("isRight", false);
                animationController.SetBool("isRunning", false);
            }
            else if (rightKey)
            {
                animationController.SetBool("isLeft", false);
                animationController.SetBool("isRight", true);
                animationController.SetBool("isRunning", false);
            }
            else
            {
                animationController.SetBool("isLeft", false);
                animationController.SetBool("isRight", false);
                animationController.SetBool("isRunning", false);
            }
        }
        else
        {
            animationController.SetBool("isLeft", false);
            animationController.SetBool("isRight", false);
            animationController.SetBool("isRunning", false);
            animationController.SetBool("isWalking", false);
            animationController.SetBool("isLeft", false);
            animationController.SetBool("isRight", false);
        }
    }
}
