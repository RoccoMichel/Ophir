using UnityEngine;

public class Ladder : Interactable
{
    [Header("Ladder Settings")]
    public float climbForce = 1f;
    public float exitOffset = 0.7f;
    [SerializeField] protected Transform Top;
    [SerializeField] protected Transform Bottom;

    protected Transform user;
    protected bool playerGravityRelation;

    protected override void OnStart()
    {
        base.OnStart();

        if (Top == null ||  Bottom == null)
        {
            Debug.LogWarning("'Top' and or 'Bottom' transform is unassigned!");
            gameObject.SetActive(false);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (active)
        {
            if (FirstPersonController.jumpAction.WasPressedThisFrame() || CameraController.interactAction.WasPressedThisFrame())
            {
                ExitLadder(player.transform.position + -transform.forward * exitOffset);
            }

            Climbing();
        }
    }

    protected virtual void Climbing()
    {
        Vector3 climbAmount = new (0, FirstPersonController.moveAction.ReadValue<Vector2>().y * climbForce * Time.deltaTime, 0);


        user.transform.position = ClampPoint(user.transform.position + climbAmount, Bottom.position, Top.position);
    }

    public virtual void EnterLadder(Transform user)
    {
        user.TryGetComponent(out FirstPersonController player);
        playerGravityRelation = player.useGravity;
        player.useGravity = false;
        this.user = user;
        active = true;
    }

    public virtual void ExitLadder(Vector3 exitPosition)
    {
        user.TryGetComponent(out FirstPersonController controller);
        controller.useGravity = playerGravityRelation;
        user.transform.position = exitPosition;
        user = null;
        active = false;
    }

    protected override void TriggerEnter(Collider collider)
    {
        base.TriggerEnter(collider);

        // don't check for trigger validation because it is always valid
        if (collider.CompareTag("Player"))
        {
            EnterLadder(collider.transform);
        }
    }

    public static Vector3 ClampPoint(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
    {
        return ClampProjection(ProjectPoint(point, segmentStart, segmentEnd), segmentStart, segmentEnd);
    }

    public static Vector3 ProjectPoint(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
    {
        return segmentStart + Vector3.Project(point - segmentStart, segmentEnd - segmentStart);
    }

    private static Vector3 ClampProjection(Vector3 point, Vector3 start, Vector3 end)
    {
        var toStart = (point - start).sqrMagnitude;
        var toEnd = (point - end).sqrMagnitude;
        var segment = (start - end).sqrMagnitude;
        if (toStart > segment || toEnd > segment) return toStart > toEnd ? end : start;
        return point;
    }
}
