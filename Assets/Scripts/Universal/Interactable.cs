using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public UnityEvent consequence;
    public bool active;
    public bool interactable;
    public ValidationMethods validation;
    public float minDistance = 5f;

    // non-inspector variables
    protected Transform player; 
    protected InputAction interactAction;

    public enum ValidationMethods { distance, trigger }

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

        // Early return if not interactable in this frame
        if (!interactable) return;
    }

    public virtual void Interaction()
    {
        active = true;
        consequence.Invoke();
    }


    // Logic if validation methods depends on Trigger:
    private void OnTriggerEnter(Collider other)
    {
        if (validation == ValidationMethods.trigger && other.CompareTag("Player"))
        {
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (validation == ValidationMethods.trigger && other.CompareTag("Player"))
        {
            interactable = false;
        }
    }
}