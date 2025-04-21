using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public UnityEvent consequence;
    public bool active;
    public bool interactable;
    public ValidationMethods validation = ValidationMethods.trigger;
    [Tooltip("Set Raycast distance on the CameraController")]
    public float minDistance = 5f;

    // non-inspector variables
    protected Transform player; 
    protected InputAction interactAction;

    public enum ValidationMethods { none, distance, trigger, raycast }

    private void Start()
    {
        OnStart();
    }
    private void Update()
    {
        OnUpdate();
    }

    // Overridable Start Method
    protected virtual void OnStart()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Overridable Update Method
    protected virtual void OnUpdate()
    {
        // Early return if already active
        if (active) return;

        // Logic if validation methods depends on distance
        if (validation == ValidationMethods.distance)
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
            if (Vector3.Distance(transform.position, player.position) < minDistance)
                interactable = true;
            else interactable = false;
        }
    }

    public virtual void Interaction()
    {
        active = true;
        consequence.Invoke();
    }

    public virtual void RaycastInteraction()
    {
        if (validation != ValidationMethods.raycast || !interactable) return;

        Interaction();
    }

    protected virtual void TriggerEnter(Collider collider)
    {
        if (validation == ValidationMethods.trigger && collider.CompareTag("Player"))
        {
            interactable = true;
        }
    }

    // Logic if validation methods depends on Trigger:
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (validation == ValidationMethods.trigger && other.CompareTag("Player"))
        {
            interactable = false;
        }
    }

    // Warn unknowing dev
    private void OnValidate()
    {
        if (validation == ValidationMethods.raycast && !gameObject.CompareTag("Interactable"))
        {
            Debug.LogWarning("Made sure to set Object tag to 'Interactable' if using raycast validation\nIT IS CURRENTLY NOT SET!");
        }
    }
}