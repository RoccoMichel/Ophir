using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public BasePlayer playerReference;
    public RangedWeapon gunReference;
    [SerializeField] internal TMP_Text healthDisplay;
    [SerializeField] internal TMP_Text ammoDisplay;

    private void Start()
    {
        if (playerReference == null)
            playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
    }

    private void Update()
    {
        if (playerReference != null)
        {
            if (playerReference.armor > 0)
                healthDisplay.text = $"{playerReference.health}\t|{playerReference.armor}";
            else
                healthDisplay.text = playerReference.health.ToString();
        }

        if (gunReference != null)
        {
            ammoDisplay.text = "0|\t0";

            ammoDisplay.text = $"{gunReference.activeAmmo}|\t{gunReference.carryingAmmo}";
        }
    }
}