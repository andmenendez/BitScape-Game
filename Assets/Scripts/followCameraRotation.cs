using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCameraRotation : MonoBehaviour
{
    public Vector2 rotate;
    public float mouseSensitivity = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {

        if (Time.timeScale == 0)
            return;

        rotate.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotate.y = Mathf.Clamp(rotate.y, -65f, 80f);
        transform.localRotation = Quaternion.Euler(-rotate.y, 0f, 0f);
    }
}