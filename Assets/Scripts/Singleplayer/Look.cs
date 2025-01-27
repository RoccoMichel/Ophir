using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public Transform player;

    public float sensitivity = 5f;
    public bool invertX, invertY, sensitivityFromPref = false;
    public string prefKey = "Sensitivity";

    [HideInInspector] public Vector2 rotation;
    [SerializeField] internal Vector2 mouseInput;
    [SerializeField] internal float horizontal;

    private float xRotation = 0f;
    private InputAction lookAction;
    
    void Start()
    {
        if (sensitivityFromPref) sensitivity = PlayerPrefs.GetFloat(prefKey, sensitivity);

        lookAction = InputSystem.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        // Read input
        mouseInput = lookAction.ReadValue<Vector2>();
        rotation = sensitivity * Time.deltaTime * new Vector2(
            invertX ? -mouseInput.x : mouseInput.x,
            !invertY ? mouseInput.y : -mouseInput.y
        );

        xRotation -= rotation.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * rotation.x);
    }
}