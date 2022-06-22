using UnityEngine;

public class PlayerController : MonoBehaviour {
    public CharacterController controller;
    public Transform groundElement;
    public float groundDistance = 0.3f;
    public LayerMask groundLayer;
    public float speed = 1f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    bool isGrounded;
    bool picked = false;
    bool picked2 = false;
    public MenuController pauseMenu;

    void Update() {
        if (Time.timeScale == 0)
            return;

        bool isGrounded = Physics.CheckSphere(groundElement.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;
        controller.Move(move * speed);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        if (Input.GetButtonDown("Fire1")) {
            picked = !picked;
        }
        if (picked) {
            GameObject.Find("battery2").transform.position = transform.position + new Vector3(1f, 1f, 1f); GameObject.Find("battery2").transform.position = transform.position + new Vector3(1f, 1f, 1f);
        }


        if (Input.GetButtonDown("Fire2")) {
            picked2 = !picked2;
        }
        if (picked2) {
            GameObject.Find("battery1").transform.position = transform.position + new Vector3(1f, 1f, 1f); GameObject.Find("battery1").transform.position = transform.position + new Vector3(1f, 1f, 1f);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
