using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputSystem_Actions inputActions;
    public float moveSpeed;
    public float rotationSpeed;
    private float mouseRotationX;
    public float topClampMouse = -90f;
    public float bottomClampMouse = 90f;
    float yRotation = 0f;
    public CharacterController controller;
    public InputAction move;
    public InputAction jump;
    public InputAction look;
    public InputAction fire;
    public GameObject projectilePrefab;
    public float damage;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float sprintMultiplier = 1.5f; 
    private float verticalVelocity;
    private bool isSprinting;      
    public Transform cameraHolder;       
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        move = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        look = inputActions.Player.Look;
        fire = inputActions.Player.Fire;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        PlayerMovement();
        MouseMovement();
        OnFire();
    }

    void PlayerMovement()
    {
        Vector2 moveInputVector = move.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        forward.y = 0f; // ignore vertical rotation
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 movement = right * moveInputVector.x + forward * moveInputVector.y;
        isSprinting = inputActions.Player.Sprint.ReadValue<float>() > 0;
        float speed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
        controller.Move(movement * speed * Time.deltaTime);
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        if (jump.triggered && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);   
        }
        
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        
    }

    void MouseMovement()
    {
        Vector2 lookInputVector = look.ReadValue<Vector2>();
        float mouseX = lookInputVector.x * rotationSpeed * 5f * Time.deltaTime;
        float mouseY = lookInputVector.y * rotationSpeed * 5f * Time.deltaTime;
    
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        mouseRotationX -= mouseY;
        mouseRotationX = Mathf.Clamp(mouseRotationX, topClampMouse, bottomClampMouse);
        cameraHolder.localRotation = Quaternion.Euler(mouseRotationX, 0f, 0f);
    }

    /*legacy projectile based shooting
    void OnFire()
    {
        if (fire.triggered && !PauseMenu.gameIsPaused)
        {
            Vector3 spawnPos = cameraHolder.position + cameraHolder.forward * 1f;
            Instantiate(projectilePrefab, spawnPos, cameraHolder.rotation);
        }
    } */

    void OnFire()
    {
        if (fire.triggered && !PauseMenu.gameIsPaused)
        {
            Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log("Hit: " + hitObject.name);

                Health targetHealth = hit.collider.GetComponentInParent<Health>();
                if (targetHealth != null && targetHealth.CompareTag("Enemy"))
                {
                    targetHealth.TakeDamage(damage);
                }
                else if (targetHealth != null)
                {
                    Debug.LogWarning("Hit a Health object but it is not tagged Enemy: " + targetHealth.gameObject.name);
                }
                else
                {
                    Debug.Log("No enemy Health found on hit object.");
                }
            }
        }
    }
}

