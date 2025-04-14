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
        if (Input.GetKeyDown(KeyCode.F)) Instance(Random.Range(0, 1000));
        if (displayValue > 0)
            amountDisplay.text = $"+ {(int)Mathf.Lerp(displayValue, 0, Mathf.Clamp01(decreaseRate += Time.deltaTime))}";
    }

    public void Instance(int amount)
    {
        displayValue = amount;
        decreaseRate = -0.5f;

        // this is retarded unity, just let me chance them like a variable
        var emission = particle.emission;
        emission.rateOverTime = amount + 1; // plus 1 cause particle effect is kind of goofy

        particle.Play();

        // turn on object (visually)
        // create effect based on amount
    }
}
