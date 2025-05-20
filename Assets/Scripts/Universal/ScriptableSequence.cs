using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScriptableSequence : MonoBehaviour
{
    [Header("Sequence Settings")]
    public GameObject target;
    public bool oneShot = true;
    private bool triggered;

    [Header("Events")]
    public UnityEvent events;

    [Header("Choose the Effect Events for these")]
    [SerializeField] GameObject[] particleEffects;
    [SerializeField] AudioClip[] soundEffects;
    [SerializeField] Animator[] animatonEffects;
    [SerializeField] string[] clipName;

    [Header("Camera Shake Event Settings")]
    public float duration;
    public float magnitude;

    private void OnTriggerEnter(Collider other)
    {
        if (oneShot && triggered) return;

        if (!other.gameObject.CompareTag("Player")) return;

        triggered = true;

        events.Invoke();
    }
    
    // EVENTS:
    public void DebugLog(string msg)
    {
        Debug.Log(msg);
    }
    public void AnimationEffects()
    {
        if (clipName.Length != animatonEffects.Length) Debug.LogWarning("Unequal amount of animations and clip names!");

        for (int i = 0; i < animatonEffects.Length; i++)
        {
            try { animatonEffects[i].Play(clipName[i]); }
            catch 
            { 
                if (clipName[i] != null) Debug.LogError($"'{clipName[i]}' is invalid for animation controler at element index '{i}'");
                else Debug.LogError($"clipName at index '{i}' is NULL!");            
            }
        }
    }
    public void SoundEffects()
    {
        AudioSource source;
        if (target.TryGetComponent(out source))
        {
            try { source = gameObject.GetComponent<AudioSource>(); }
            catch { source = gameObject.AddComponent<AudioSource>(); }
        }

        foreach (AudioClip clip in soundEffects)
        {
            source.PlayOneShot(clip);
        }
    }
    public void CameraShake()
    {
        StartCoroutine(Camera.main.GetComponent<CameraController>().Shake(duration, magnitude));
    }
    public void ParticleEffects()
    {
        Transform location = target == null ? transform : target.transform;

        foreach (var effect in particleEffects)
        {
            // Check if it is already in scene
            if (effect.scene.name == null) Instantiate(effect, location.position, location.rotation).GetComponent<ParticleSystem>().Play();
            else effect.GetComponent<ParticleSystem>().Play();
        }
    }
    public void LoadNextScene()
    {
        try { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
        catch { Debug.LogError("Failed Loading Next Scene"); }
    }    
    public void LoadPreviousScene()
    {
        try { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); }
        catch { Debug.LogError("Failed Loading Previous Scene"); }
    }
    public void LoadIndexScene(int index)
    {
        try { SceneManager.LoadScene(index); }
        catch { Debug.LogError($"Failed Loading Scene with buildIndex '{index}'"); }
    }    
    public void LoadNameScene(string name)
    {
        try { SceneManager.LoadScene(name); }
        catch { Debug.LogError($"Failed Loading Scene with name '{name}'"); }
    }
}
