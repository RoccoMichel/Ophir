using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The Canvas damage indication will be displayed on")]
    public GameObject targetCanvas;

    [Tooltip("Indicator Prefab that gets spawned in, NEEDS \'Indicator.cs\'")]
    public GameObject indicator;

    private void Start()
    {
        // Setting targetCanvas depending on situation
        if (targetCanvas == null)
        {
            if (gameObject.GetComponent<Canvas>() != null) targetCanvas = gameObject;
            else if (FindAnyObjectByType<Canvas>() != null) targetCanvas = FindAnyObjectByType<Canvas>().gameObject;
            else
            {
                gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                gameObject.AddComponent<GraphicRaycaster>();
                targetCanvas = gameObject;

                Debug.LogWarning($"No Canvas found in current Scene!\nAdded Temporary to \"{gameObject.name}\"");
            }        
        }

        // If targetCanvas was only a Prefab and not in the scene, welp, not anymore!
        if (targetCanvas.scene.name == null) targetCanvas = Instantiate(targetCanvas);

        // If Indicator Prefab is missing it will try to get it from the content browser | ? WILL NOT WORK IN BUILD
        if (indicator == null)
        {
            Debug.LogWarning("Indicator GameObject is missing Reference!\nAttempting to find in Assets drawer...");
            
            indicator = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Damage Indicator/Indicator PREFAB.prefab");

            if (indicator == null) Debug.LogWarning("[Indicator PREFAB] has been MOVED or RENAMED!\nCode fix required at Ln:41!");
            else Debug.Log("Successfully found [Indicator PREFAB] in Content Browser\nHowever manual assignment might be required for build!");
        }
    }

    // CREATE A NEW INDICATOR

    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>
    public void InstantiateIndicator(Transform target)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.target = target;
    }
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>
    /// <param name="followTarget">To either follow target actively or lock on initial location</param>
    public void InstantiateIndicator(Transform target, bool followTarget)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.target = target;
        newIndicator.followTarget = followTarget;
    }
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>
    /// <param name="distanceFromPivot">Distance the Indicator will have from the center of the screen</param>
    public void InstantiateIndicator(Transform target, float distanceFromPivot)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.target = target;
        newIndicator.distanceFromPivot = distanceFromPivot;
    }
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>
    /// <param name="followTarget">To either follow target actively or lock on initial location</param>
    /// <param name="distanceFromPivot">Distance the Indicator will have from the center of the screen</param>
    /// <param name="duration">The length of time (seconds) the indicator will exist</param>
    public void InstantiateIndicator(Transform target, bool followTarget, float distanceFromPivot, float duration)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.target = target;
        newIndicator.distanceFromPivot = distanceFromPivot;
        newIndicator.followTarget = followTarget;
        newIndicator.durationSeconds = duration;
    }
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>
    /// <param name="color">Shade of UI element</param>
    public void InstantiateIndicator(Transform target, Color color)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.target = target;
        newIndicator.color = color;
    }
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>    
    /// <param name="followTarget">To either follow target actively or lock on initial location</param>
    /// <param name="distanceFromPivot">Distance the Indicator will have from the center of the screen</param>
    /// <param name="color">Shade of UI element</param>
    public void InstantiateIndicator(Transform target, bool followTarget, float distanceFromPivot, Color color)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.distanceFromPivot = distanceFromPivot;
        newIndicator.target = target;
        newIndicator.color = color;
        newIndicator.followTarget = followTarget;
    }    
    /// <summary>
    /// Instantiate a 2D UI Indicator towards a 3D location
    /// </summary>
    /// <param name="target">3D location the Indicator will try to aim towards</param>    
    /// <param name="followTarget">To either follow target actively or lock on initial location</param>
    /// <param name="distanceFromPivot">Distance the Indicator will have from the center of the screen</param>
    /// <param name="duration">The length of time (seconds) the indicator will exist</param>
    /// <param name="color">Shade of UI element</param>
    public void InstantiateIndicator(Transform target, bool followTarget, float distanceFromPivot, float duration, Color color)
    {
        Indicator newIndicator = Instantiate(indicator, targetCanvas.transform).GetComponent<Indicator>();
        newIndicator.distanceFromPivot = distanceFromPivot;
        newIndicator.target = target;
        newIndicator.color = color;
        newIndicator.followTarget = followTarget;
        newIndicator.durationSeconds = duration;
    }
}