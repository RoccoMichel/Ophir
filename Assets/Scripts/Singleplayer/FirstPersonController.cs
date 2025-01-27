using Photon.Realtime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
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
    }
}