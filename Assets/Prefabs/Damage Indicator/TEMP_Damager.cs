using UnityEngine;

public class TEMP_Damager : MonoBehaviour
{
    [Header("Click bool to TEST")]
    public bool doDamage = false;

    [Header("Configurations")]
    public float distanceFromPivot = 5;
    public float duration = 2;
    public Color color = Color.white;
    public bool followTarget = true;

    private void Update()
    {
        if (doDamage)
        {
            FindAnyObjectByType<DamageIndicator>().InstantiateIndicator(transform, followTarget, distanceFromPivot, duration, color);
            doDamage = false;
        }
    }
}
