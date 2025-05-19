using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // Quand l'action "Move" est utilisée
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Quand l'action "Look" est utilisée
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked; // Cache et bloque le curseur
    }

    void OnDisable()
    {
        controls.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotation horizontale (joueur)
        transform.Rotate(Vector3.up * mouseX);

        // Rotation verticale (caméra)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
