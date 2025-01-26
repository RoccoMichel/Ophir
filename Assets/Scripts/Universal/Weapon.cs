using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public bool debug = false;
    /// <summary>
    /// Weapon name
    /// </summary>
    [Header("Weapon Settings")]
    public string identity = "Unnamed Weapon";
    public float damage = 10f;
    public float distance = 50f;
    public float fovKickback = 0f;

    protected InputAction attackAction;
    protected float timeSinceLastShot = 0f;

    private void Start()
    {
        VariableAssignment();
    }

    private void Update()
    {
        OnUpdate();
    }

    // WEAPON METHODS ------------------------

    public virtual void DoDamage(Entity target)
    {
        target.TakeDamage(damage);
    }

    public virtual void VariableAssignment()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    public virtual void OnUpdate()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}