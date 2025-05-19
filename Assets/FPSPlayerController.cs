using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public float speed = 5f;                   // Vitesse de d�placement
    public float mouseSensitivity = 2f;        // Sensibilit� de la souris
    public float gravity = -9.81f;             // Force de gravit�
    public float jumpHeight = 1.5f;            // Hauteur de saut
    public Transform playerCamera;             // Cam�ra du joueur (assign�e dans l'inspecteur)

    private CharacterController controller;
    private float verticalRotation = 0f;       // Pour limiter la rotation verticale de la cam�ra
    private Vector3 velocity;                  // Vitesse actuelle (y = gravit�/saut)
    private bool isGrounded;                   // Pour v�rifier si le joueur est au sol

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;  // Verrouiller le curseur
    }

    void Update()
    {
        // --- Contr�le de la souris ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // --- Mouvement (WASD) ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // --- V�rification sol ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Pour garder le joueur au sol
        }

        // --- Saut ---
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Gravit� ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
