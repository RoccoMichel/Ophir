using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageDecay : MonoBehaviour
{
    public float messageLife;

    private void Start()
    {
        Destroy(gameObject, messageLife);

        TMP_Text[] elements = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in elements) text.CrossFadeAlpha(0, messageLife, false);
        GetComponentInChildren<Image>().CrossFadeAlpha(0, messageLife, false);
    }
}
