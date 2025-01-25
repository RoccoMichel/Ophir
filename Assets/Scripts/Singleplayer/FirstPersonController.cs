using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("TEMP")]
    public float sensitivity = 5;

    [Header("Properties")]
    public float moveSpeed;
    public float jumpStrength;
    public float gravity;
    public bool grounded;
    public float groundDistance;
    public LayerMask groundMask;

    Vector3 velocity;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public GameObject playerCamera;

    internal InputAction moveAction;
    internal InputAction jumpAction;
    internal InputAction lookAction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;

        if (grounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;

        if (jumpAction.IsPressed() && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2 * gravity);
        }

        controller.Move(velocity * Time.deltaTime);
        controller.Move(moveSpeed * Time.deltaTime * move);

        // Camera
        Vector2 lookValue = sensitivity * Time.deltaTime * lookAction.ReadValue<Vector2>();

        GetComponent<Camera>().transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(-lookValue.y, -90f, 90f), 0f, 0f) + GetComponent<Camera>().transform.localRotation.eulerAngles);
        transform.Rotate(Vector3.up * lookValue.x);

    }
}