using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] internal TMP_Text healthDisplay;
    [SerializeField] internal TMP_Text ammoDisplay;
    public BasePlayer playerReference;
    public RangedWeapon gunReference;

    [Header("Weapon UI")]
    public RectTransform weaponSelector; // max 6 weapons with current layout config, else it goes beyond the layout group
    [SerializeField] internal List<RectTransform> weapons = new();
    [SerializeField] internal RectTransform template;
    [SerializeField] internal RectTransform selector;


    private void Start()
    {
        if (playerReference == null)
            playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();    
    }

    private void Update()
    {
        // Set Health / Armor Display
        if (playerReference != null)
        {
            if (playerReference.armor > 0)
                healthDisplay.text = $"{playerReference.health}\t|{playerReference.armor}";
            else
                healthDisplay.text = playerReference.health.ToString();
        }

        // Set Ammo Display
        if (gunReference != null)
        {
            ammoDisplay.text = "0|\t0";

            ammoDisplay.text = $"{gunReference.activeAmmo}|\t{gunReference.carryingAmmo}";
        }
    }

    public void Refresh(Inventory inventory)
    {
        foreach(RectTransform transform in weapons)
            Destroy(transform.gameObject);
        weapons.Clear();

        for (int i = 0; i < inventory.weapons.Count; i++)
        {
            template.gameObject.SetActive(true);
            weapons.Add(Instantiate(template, template.parent));
            template.gameObject.SetActive(false);
        }
    }

    public void SetSelector(int index)
    {
        SetSelector(true);
        selector.position = weapons[index].position;
        selector.localScale = weapons[index].localScale;
    }

    public void SetSelector(bool value)
    {
        selector.gameObject.SetActive(value);
    }

    public void NewAmmo(int index, int amount)
    {
        if (amount <= 0) return;
        weapons[index].GetComponentInChildren<NewAmmo>().Instance(amount);
    }
}