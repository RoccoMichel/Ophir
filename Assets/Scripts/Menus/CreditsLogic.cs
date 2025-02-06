using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsLogic : MonoBehaviour
{
    public bool isPlaying = true;
    public float durationSeconds = 10f;
    public float timer;
    public RectTransform creditsObject;
    InputAction pauseAction;

    private void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        if (creditsObject == null) return;

        if (pauseAction.WasPressedThisFrame()) isPlaying = !isPlaying;

        if (isPlaying) timer += Time.deltaTime;

        // Move transform vertically based on its height and the timer
        creditsObject.position = new Vector2(
            creditsObject.position.x,
            Mathf.Lerp(-creditsObject.sizeDelta.y / 2, creditsObject.sizeDelta.y / 2,
            Mathf.InverseLerp(0, durationSeconds, timer)));

        if (timer > durationSeconds) LoadLogic.LoadSceneByNumber(0);
    }
}