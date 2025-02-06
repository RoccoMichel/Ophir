using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public bool followTarget;
    public float distanceFromPivot;
    public float durationSeconds = 1;

    public Color color = Color.white;
    public Transform target;
    public Transform player;

    Transform origin;
    Image image;
    RectTransform indicationTransform;

    void Awake()
    {
        // Important References
        image = GetComponentInChildren<Image>();
        indicationTransform = GetComponent<RectTransform>();

        try { player = GameObject.FindGameObjectWithTag("Player").transform; } 
        catch {
            Debug.LogWarning($"No Player in current Context!\nPlayer has been set to \"{FindFirstObjectByType<DamageIndicator>().name}\"");
            player = FindFirstObjectByType<DamageIndicator>().transform;
        }

        origin = new GameObject("[TEMP] Indicator Origin").GetComponent<Transform>();
    }
    private void Start()
    {
        // Trust me, it's for the best
        distanceFromPivot = Mathf.Abs(distanceFromPivot);
        if (distanceFromPivot == 0) distanceFromPivot = 5;
        if (durationSeconds == 0) durationSeconds = 1;


        // Set values on the first frame cause they don't wanna in Awake()
        image.color = color;
        GetComponent<KillAfter>().lifeTimeSeconds = durationSeconds;
        Destroy(origin.gameObject, durationSeconds);
        image.GetComponent<RectTransform>().position -= new Vector3(0, distanceFromPivot * 1000, 0);
        origin.SetPositionAndRotation(target.position, target.rotation);

        // Fade out
        image.CrossFadeAlpha(0, durationSeconds, true);
    }

    void Update()
    {
        // Fix itself when critical elements are missing
        if (player == null) Destroy(gameObject);
        if (target == null) followTarget = false;

        // Rotate to Target depending on target, 
        if (followTarget) RotateTowards(target);
        else RotateTowards(origin);
    }

    void RotateTowards(Transform target) // Thanks Indian YouTube Guy lmfao
    {
        if (target == null) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(player.position - target.position);
        targetRotation.z = -targetRotation.y;
        targetRotation.x = 0;
        targetRotation.y = 0;

        Vector3 playerNorth = new(0,0, player.eulerAngles.y);

        indicationTransform.localRotation = targetRotation * Quaternion.Euler(playerNorth);
    }
}