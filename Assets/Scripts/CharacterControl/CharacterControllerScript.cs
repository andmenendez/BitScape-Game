using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class CharacterControllerScript : MonoBehaviour {
    private CharacterController characterController;
    public Transform cameraController;
    public Transform groundElement;
    public LayerMask groundLayer;
    public float groundDistance = 0.5f;

    private float dx = 0;
    private float dz = 0;

    public float gravity = -18f;
    public float walkSpeed = 6f;
    public float runSpeed = 15f;
    public float acceleration = 1;
    public float jumpHeight = 3f;
    public float rotationDamper = 0.5f; // reduces the rate of turn
    public float backwardsTurnRateDeg = 15;
    public float idleTurnRateDeg = 20;

    public bool isGrounded;
    private bool disolved = false;
    private bool resolved = false;
    float moveSpeed;

    private AudioSource audioSource;
    private AudioClip disolveFx;
    private AudioClip resolveFx;

    public Vector2 rotation;
    public float mouseSensitivity = 3.0f;

    Animator m_Animator;
    Vector3 velocity = Vector3.zero;
    float verticalVelocity = 0f;
    

    public void footstepAudioEvent() {
        LevelState.Instance.PlaySound("footstep", gameObject, 0.3f);
    }

    public void jumpLandingAudioEvent() {
        LevelState.Instance.PlaySound("jump_landing", gameObject, 0.3f);
    }

    void Start() {
        m_Animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        disolveFx = (AudioClip)Resources.Load($"Sounds/teleport_0");
        resolveFx = (AudioClip)Resources.Load($"Sounds/teleport_0_rev");

        StartCoroutine(Resolve());
    }

    void Update() {
        if (Time.timeScale == 0)
            return;

        m_Animator.SetBool("isJumping", false);

        isGrounded = characterController.isGrounded;

        verticalVelocity += gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0) {
            verticalVelocity = -1f;
        }
        if (Input.GetButtonDown("Jump") && isGrounded) {
            m_Animator.SetBool("isJumping", true);
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
            if (hit.transform.CompareTag("Elevator")) {
                Disolve("Level2");
            } else if (hit.transform.CompareTag("Elevator2")) {
                Disolve("Level3");
            } else if (hit.transform.CompareTag("Elevator3")) {
                Disolve("Start");
            }
        }

        if (Input.GetButton("Fire3")) {
            moveSpeed = runSpeed;
            m_Animator.SetBool("isRunning", true);
        } else {
            moveSpeed = walkSpeed;
            m_Animator.SetBool("isRunning", false);
        }

        rotation.y += Input.GetAxis("Mouse X") * LevelState.Instance.MouseSpeed;

        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        // Input [-1,1] for left ('a') or right ('d')
        dx = Mathf.Clamp(
            dx + acceleration * Input.GetAxisRaw("Horizontal") + (Input.GetAxis("Mouse X") * LevelState.Instance.MouseSpeed) * acceleration / 2.0f,
            -1.0f, 1.0f);
        if (Input.GetAxisRaw("Horizontal") == 0) {
            dx += (0f - dx) * 0.25f;
        }

        // Input [-1,1] for for back ('s') or forwards ('w')
        dz = Mathf.Clamp(
            dz + acceleration * Input.GetAxisRaw("Vertical"),
            -1.0f, 1.0f);

        if (Input.GetAxisRaw("Vertical") == 0) {
            dz += (0f - dz) * 0.25f;
        }

        if (m_Animator.GetBool("isCarryingBox")) {
            m_Animator.SetFloat("X", dx);

        } else {
            m_Animator.SetFloat("X", dx);
        }
        m_Animator.SetFloat("Z", dz);

        velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * 0.5f, 0.0f, Input.GetAxis("Vertical") * moveSpeed);

        velocity.y = verticalVelocity;
        characterController.Move(transform.rotation * velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Z)) {
            Resolve();
        }
    }

    void Disolve(string scene)
    {

        if (disolved && !transform.Find("Character/Ch48").GetComponent<Animation>().isPlaying)
            SceneManager.LoadScene(scene);

        audioSource.loop = false;
        audioSource.PlayOneShot(disolveFx, 1.0F);

        if (disolved)
            return;
        // AUDIO QUEUE


        var go = transform.Find("Character/Ch48").gameObject;
        var skin = go.GetComponent<SkinnedMeshRenderer>();
        var bodyDisolve = (Material)Resources.Load("Materials/Player/Ch48_body_disolve", typeof(Material));
        var armorDisolve = (Material)Resources.Load("Materials/Player/Ch48_armor_disolve", typeof(Material));
        var hairDisolve = (Material)Resources.Load("Materials/Player/Ch48_hair_disolve", typeof(Material));
        var materials = skin.materials;
        materials[0] = armorDisolve;
        materials[1] = bodyDisolve;
        skin.materials = materials;

        go.GetComponent<Animation>()["disolve"].speed = 0.4f;
        go.GetComponent<Animation>().enabled = true;
        go.GetComponent<Animation>().wrapMode = WrapMode.Once;
        go.GetComponent<Animation>().Play();

        go = transform.Find("Character/Ch48_hair1").gameObject;
        skin = go.gameObject.GetComponent<SkinnedMeshRenderer>();
        materials = skin.materials;
        materials[0] = hairDisolve;
        skin.materials = materials;
        go.GetComponent<Animation>()["disolve"].speed = 0.4f;
        go.GetComponent<Animation>().enabled = true;
        go.GetComponent<Animation>().wrapMode = WrapMode.Once;
        go.GetComponent<Animation>().Play();
        disolved = true;
    }


    IEnumerator Resolve()
    {
        var go = transform.Find("Character/Ch48").gameObject;
        var skin = go.GetComponent<SkinnedMeshRenderer>();
        var bodyDisolve = (Material)Resources.Load("Materials/Player/Ch48_body_disolve", typeof(Material));
        var armorDisolve = (Material)Resources.Load("Materials/Player/Ch48_armor_disolve", typeof(Material));
        var hairDisolve = (Material)Resources.Load("Materials/Player/Ch48_hair_disolve", typeof(Material));
        var materials = skin.materials;
        var bodyMaterials = skin.materials;
        materials[0] = armorDisolve;
        materials[1] = bodyDisolve;
        skin.materials = materials;

        var anim1 = go.GetComponent<Animation>();
        anim1["resolve"].speed = 0.5f;
        anim1.enabled = true;
        anim1.Play("resolve");
        anim1.Stop();

        go = transform.Find("Character/Ch48_hair1").gameObject;
        var anim2 = go.GetComponent<Animation>();
        skin = go.gameObject.GetComponent<SkinnedMeshRenderer>();
        materials = skin.materials;
        var hairMaterials = skin.materials;
        materials[0] = hairDisolve;
        skin.materials = materials;
        anim2["resolve"].speed = 0.5f;
        anim2.enabled = true;
        anim2.Play("resolve");
        anim2.Stop();

        yield return new WaitForSeconds(0.2f);

        anim1.Play("resolve");
        anim2.Play("resolve");
        audioSource.loop = false;
        audioSource.PlayOneShot(disolveFx, 1.0F);

        yield return new WaitForSeconds(2f);
        transform.Find("Character/Ch48").gameObject.GetComponent<SkinnedMeshRenderer>().materials = bodyMaterials;
        transform.Find("Character/Ch48_hair1").gameObject.GetComponent<SkinnedMeshRenderer>().materials = hairMaterials;
    }

}
