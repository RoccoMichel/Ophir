using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] internal TMP_Text healthDisplay;
    [SerializeField] internal TMP_Text ammoDisplay;
    [SerializeField] internal Image crosshair;
    public BasePlayer playerReference;
    public RangedWeapon gunReference;

    [Header("Weapon UI")]
    public RectTransform group; // max 6 weapons with current layout config, else it goes beyond the layout group
    [SerializeField] internal List<RectTransform> weapons = new();
    [SerializeField] internal RectTransform template;
    [SerializeField] internal RectTransform selector;


    private void Start()
    {
        if (selector != null) SetSelector(false);

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
        if (gunReference != null) ammoDisplay.text = $"{gunReference.activeAmmo}|\t{gunReference.carryingAmmo}";
        else ammoDisplay.text = string.Empty;
    }

    public void Refresh(Inventory inventory)
    {
        if (group == null)
            throw new MissingReferenceException("'group' transform is not assigned!");

        if (template == null)
            throw new MissingReferenceException("'template' is not assigned!");


        foreach (RectTransform transform in weapons)
            Destroy(transform.gameObject);
        weapons.Clear();

        for (int i = 0; i < inventory.weapons.Count; i++)
        {
            weapons.Add(Instantiate(template, group));

            Weapon w = inventory.weapons[i].GetComponent<Weapon>();
            weapons[i].GetComponent<Image>().sprite = w.icon;
            weapons[i].name = w.identity + " (Icon)"; // setting hierarchy name
        }
    }

    public void SetSelector(int index)
    {
        if (selector == null || weapons[index] == null) return;

        SetSelector(true);
        selector.position = weapons[index].position;
        selector.localScale = weapons[index].localScale;
    }

    public void SetSelector(bool value)
    {
        if (selector == null) return;

        selector.gameObject.SetActive(value);
    }

    /// <summary>
    /// Set the visual weapon icon group
    /// </summary>
    /// <param name="value">to display or not</param>
    public void SetWeapons(bool value)
    {
        if (group == null) return;

        group.gameObject.SetActive(value);
    }

    public void NewAmmo(int index, int amount)
    {
        if (amount <= 0) return;

        weapons[index].GetComponentInChildren<NewAmmo>().Instance(amount);
    }

    public void SetCrosshair(Sprite sprite)
    {
        crosshair.enabled = sprite == null ? false : true;
        crosshair.sprite = sprite;
    }
}