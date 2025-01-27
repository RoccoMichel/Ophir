using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int targetFrameRate = 60;
    public bool debug = false;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 1;
    }

    private void OnGUI()
    {
        if (debug)
        {
            GUI.Label(new Rect(10, 10, 100, 100), $"FPS: {1.0f / Time.deltaTime}");
        }
    }
}
