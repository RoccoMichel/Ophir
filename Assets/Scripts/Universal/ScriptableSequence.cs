using UnityEngine;

public class ScriptableSequence : MonoBehaviour
{
    [Header("Sequence Settings")]
    public bool oneShot = true;
    private bool triggered;

    [Header("Camera Shake")]
    public bool active;
    public float duration;
    public float magnitude;

    private void OnTriggerEnter(Collider other)
    {
        if (oneShot && triggered) return;

        if (!other.gameObject.CompareTag("Player")) return;

        triggered = true;

        if (active)
        {
            StartCoroutine(Camera.main.GetComponent<CameraController>().Shake(duration, magnitude));
        }
    }
}
