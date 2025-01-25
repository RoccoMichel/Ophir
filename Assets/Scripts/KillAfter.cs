using UnityEngine;

public class KillAfter : MonoBehaviour
{
    public float lifeTimeSeconds;

    [Header("EXTRAS")]
    public GameObject[] destroyEffects;
    public AudioClip[] soundEffects;
    public Transform effectLocation;
    void Start()
    {
        Destroy(gameObject, lifeTimeSeconds);
    }

    private void OnDestroy()
    {
        foreach (GameObject effect in destroyEffects) Instantiate(effect, effectLocation != null ? effectLocation.position : transform.position, Quaternion.identity);

        foreach (AudioClip clip in soundEffects) Camera.main.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}