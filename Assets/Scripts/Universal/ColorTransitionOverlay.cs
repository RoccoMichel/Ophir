using UnityEngine;
using UnityEngine.UI;

public class ColorTransitionOverlay : MonoBehaviour
{
    public bool destroyWhenFinished = true;
    public bool ignoreTimeScale = false;
    public float transitionTimeSeconds = 1f;
    public Color startColor = Color.black;
    public Color endColor = Color.white;
    public Image component;

    private void Start()
    {
        if (component == null && !TryGetComponent(out component))
        {
            Debug.LogWarning("Transition Overlay is missing Image Component Reference");
            return;
        }

        component.raycastTarget = true;
        component.canvasRenderer.SetColor(startColor);
        component.CrossFadeColor(endColor, transitionTimeSeconds, ignoreTimeScale, true);

        if (destroyWhenFinished) Destroy(gameObject, transitionTimeSeconds);
    }
}