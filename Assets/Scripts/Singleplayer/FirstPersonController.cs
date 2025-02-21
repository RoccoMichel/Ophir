using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float acceleration = 7f;
    public float deceleration = 10f;
    protected float speed;
    internal Vector2 moveValue;
    [Header("Jumping")]
    public float jumpStrength;
    public float gravity;
    public bool grounded;
    public float groundDistance;
    public LayerMask groundMask;
    Vector3 velocity;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;

    internal InputAction moveAction;
    internal InputAction jumpAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // Movement
        if (moveAction.IsPressed())
        {
            speed += acceleration * Time.deltaTime;
            moveValue = moveAction.ReadValue<Vector2>();
        }
        else speed -= deceleration * Time.deltaTime;
        speed = Mathf.Clamp01(speed);

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 move = moveSpeed * speed * (transform.right * moveValue.x + transform.forward * moveValue.y);

        if (grounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;

        if (jumpAction.IsPressed() && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2 * gravity);
        }

        controller.Move(Time.deltaTime * (velocity + move));
    }
}