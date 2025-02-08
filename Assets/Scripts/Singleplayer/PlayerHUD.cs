using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Range(0f, 1f)]
    public float selectorSlideSpeed;
    [Header("References")]
    public RectTransform weaponSelector;
    public Inventory inventory;
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

        if (weaponSelector != null && inventory != null)
        {
            float targetPosition = inventory.cycleIndex * -100; // final position
            targetPosition = Mathf.Lerp(weaponSelector.anchoredPosition.x, targetPosition, selectorSlideSpeed); // position smoothed out

            weaponSelector.anchoredPosition = new Vector2 (targetPosition, weaponSelector.anchoredPosition.y);
        }
    }
}