using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 5f;
    public bool invertX, invertY, sensitivityFromPref = false;
    public string prefKey = "Sensitivity";

    [Header("Constraints")]
    public bool alwaysUpdate = false;
    [Tooltip("Highly recommended to avoid unwanted tilts or drifts!")]
    public bool lockZAxis = true;
    public CursorLockMode cursorConstraint;
    public MovementType moveConstraint;    
    public Vector2 limit;    
    protected Vector3 startOrientation;
    internal Vector4 activeLimit;        // x: min, max, y: min, max
    [Range(0f, 1f)] public float elasticStrength = 0.3f;

    [Header("Inputs")]
    [SerializeField] internal Vector2 mouseInput;
    [SerializeField] protected float horizontal, vertical;
    [HideInInspector] public Vector2 rotation;
    protected InputAction lookAction;

    public enum MovementType
    {
        /// <summary>
        /// Unrestricted movement. The camera can move anywhere.
        /// </summary>
        Unrestricted,

        /// <summary>
        /// Restricted movement. The camera can move anywhere but not overturn itself (barrel roll)
        /// </summary>
        Restricted,

        /// <summary>
        /// Elastic movement. The camera is allowed to temporarily move beyond the limiting Vector, but is pulled back elastically.
        /// </summary>
        Elastic,

        /// <summary>
        /// Clamped movement. The camera can not be moved beyond its limiting Vector.
        /// </summary>
        Clamped,
    }

    private void Start()
    {
        // Set values to beyond assembly variables
        lookAction = InputSystem.actions.FindAction("Look");
        startOrientation = transform.rotation.eulerAngles;

        // Apply values to scene
        Cursor.lockState = cursorConstraint;

        // Use Methods built-in conflict detection
        SetMoveConstraint(moveConstraint, limit, true);
        if (moveConstraint == MovementType.Elastic && !alwaysUpdate)
            Debug.LogWarning("It's highly recommended to set alwaysUpdate to true when constraint mode is elastic!");
    }

    private void Update()
    {
        if (!lookAction.IsPressed() && !alwaysUpdate) return;

        // Read input
        mouseInput = lookAction.ReadValue<Vector2>();
        rotation = sensitivity * Time.deltaTime * new Vector2(
            invertX ? -mouseInput.x : mouseInput.x,
            !invertY ? mouseInput.y : -mouseInput.y
        );

        // Update horizontal and vertical
        horizontal += rotation.x;
        vertical -= rotation.y;

        // Apply constraints
        if (moveConstraint == MovementType.Restricted)
        {
            vertical = Mathf.Clamp(vertical, -90f, 90f);
        }
        else if (moveConstraint == MovementType.Clamped)
        {
            vertical = Mathf.Clamp(vertical, activeLimit.z, activeLimit.w);
            horizontal = Mathf.Clamp(horizontal, activeLimit.x, activeLimit.y);
        }
        else if (moveConstraint == MovementType.Elastic)
        {
            if (vertical > activeLimit.w) vertical = Mathf.Lerp(vertical, activeLimit.w, elasticStrength);
            else if (vertical < activeLimit.z) vertical = Mathf.Lerp(vertical, activeLimit.z, elasticStrength);

            if (horizontal > activeLimit.y) horizontal = Mathf.Lerp(horizontal, activeLimit.y, elasticStrength);
            else if (horizontal < activeLimit.x) horizontal = Mathf.Lerp(horizontal, activeLimit.x, elasticStrength);
        }

        // Apply rotation using Quaternion to avoid gimbal lock
        transform.rotation = Quaternion.Euler(vertical, horizontal, lockZAxis ? 0 : transform.rotation.eulerAngles.z);
    }

    // SCRIPT METHODS ---------------------------------

    /// <summary>
    /// Sets the scripts sensitivity based on CameraController.getFromPref : bool
    /// </summary>
    public void RefreshSensitivity()
    {
        if (sensitivityFromPref) sensitivity = PlayerPrefs.GetFloat(prefKey, 1);
    }

    /// <summary>
    /// Overrides CameraController.sensitivity : float to any desired
    /// </summary>
    /// <param name="f">New sensitivity value</param>
    public void RefreshSensitivity(float f)
    {
        sensitivity = f;
    }

    /// <summary>
    /// Set objects movement constraint
    /// </summary>
    /// <param name="type">new constraint</param>
    public void SetMoveConstraint(MovementType type)
    {
        moveConstraint = type;

        // Warn about variable conflict
        if (limit != Vector2.zero && moveConstraint != MovementType.Unrestricted)
        {
            Debug.LogWarning("Limit will not be applied due to chosen containment Type");
        }
    }

    /// <summary>
    /// Set objects movement constraint & view limit
    /// </summary>
    /// <param name="type">new constraint</param>
    /// <param name="maxLimit">Up and Down max limit in Euler angles</param>
    public void SetMoveConstraint(MovementType type, Vector2 maxLimit)
    {
        if (limit.y > 90) Debug.LogWarning("Y Limit can not be beyond 90");

        limit = new Vector2(limit.x, Mathf.Clamp(limit.y, 0, 90));
        moveConstraint = type;
        limit = maxLimit;

        // Calculate Boundaries
        if (moveConstraint != MovementType.Unrestricted)
        {
            activeLimit = new Vector4 (startOrientation.y - limit.x, startOrientation.y + limit.x, startOrientation.x - limit.y, startOrientation.x + limit.y);
        }

        // Warn about variable conflict
        if (limit != Vector2.zero && moveConstraint == MovementType.Unrestricted)
        {
            Debug.LogWarning("Limit will not be applied due to chosen containment Type");
        }

        if (limit.x < 0 || limit.y < 0)
        {
            limit = new Vector2(Mathf.Abs(limit.x), Mathf.Abs(limit.y));
            Debug.LogWarning("Limit was set to negative, value has been set to absolute");
        }
    }

    /// <summary>
    /// Set objects movement constraint & view limit
    /// </summary>
    /// <param name="type">new constraint</param>
    /// <param name="maxLimit">Up and Down max limit in Euler angles</param>
    /// <param name="b">Overwrite startOrientation : Vector3</param>
    public void SetMoveConstraint(MovementType type, Vector2 maxLimit, bool b)
    {
        if (limit.y > 90) Debug.LogWarning("Y Limit can not be beyond 90");

        limit = new Vector2(limit.x, Mathf.Clamp(limit.y, 0, 90));
        moveConstraint = type;
        limit = maxLimit;

        // Calculate Boundaries
        if (b) startOrientation = transform.rotation.eulerAngles;

        if (moveConstraint != MovementType.Unrestricted)
        {
            activeLimit = new Vector4(startOrientation.y - limit.x, startOrientation.y + limit.x, startOrientation.x - limit.y, startOrientation.x + limit.y);
        }

        // Warn about variable conflict
        if (limit != Vector2.zero && moveConstraint == MovementType.Unrestricted)
        {
            Debug.LogWarning("Limit will not be applied due to chosen constraint Type");
        }

        if (limit.x < 0 || limit.y < 0)
        {
            limit = new Vector2(Mathf.Abs(limit.x), Mathf.Abs(limit.y));
            Debug.LogWarning("Limit was set to negative, value has been set to absolute");
        }
    }
}