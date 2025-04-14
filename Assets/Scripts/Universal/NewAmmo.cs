using TMPro;
using UnityEngine;

public class NewAmmo : MonoBehaviour
{
    [SerializeField] TMP_Text amountDisplay;
    [SerializeField] ParticleSystem particle;
    float displayValue;
    float decreaseRate;

    void Update()
    {
        // display amount while ticking down over time
        if (displayValue > 0)
            amountDisplay.text = $"+ {Mathf.Ceil(Mathf.Lerp(displayValue, 0, Mathf.Clamp01(decreaseRate += Time.deltaTime)))}";
    }

    public void Instance(int amount)
    {
        displayValue = amount;
        decreaseRate = -0.5f;

        // this is retarded unity, just let me chance them like a variable
        var emission = particle.emission;
        emission.rateOverTime = amount + 1; // plus 1 cause particle effect is kind of goofy

        amountDisplay.GetComponent<Animator>().Play("Full");
        particle.Play();
    }
}
