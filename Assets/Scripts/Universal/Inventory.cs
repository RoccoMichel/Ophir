using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static string weaponResourcesPath = "Singleplayer/Weapons/";
    public bool canCycle = true;
    public int activeIndex;
    public int cycleIndex;
    public float cycleTime = 1f;
    public List<GameObject> weapons;
    public PlayerHUD hud;

    internal bool cycling = false;
    internal InputAction cycleAction;
    internal InputAction confirmAction;

    private float timer = 0;

    private void Start()
    {
        cycleAction = InputSystem.actions.FindAction("Cycle");
        confirmAction = InputSystem.actions.FindAction("Attack");

        if (hud == null)
        {
            try { hud = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerHUD>(); } 
            catch { hud = FindAnyObjectByType<PlayerHUD>().GetComponent<PlayerHUD>(); }
            finally { hud.Refresh(this); }
        }
    }

    private void Update()
    {
        // Cycling weapons
        if (canCycle && cycleAction.WasPressedThisFrame())
        {
            cycleIndex += (int)cycleAction.ReadValue<float>();

            if (cycleIndex < 0) cycleIndex = Mathf.Clamp(weapons.Count - 1, 0, weapons.Count);
            if (cycleIndex > weapons.Count - 1) cycleIndex = 0;

            cycling = true;
            timer = cycleTime;

            hud.SetSelector(cycleIndex);
        }

        // Controlling the Cycling Period
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0) StopCycling();
        }

        if (cycling && confirmAction.WasPressedThisFrame())
            SwitchWeapon(cycleIndex);
    }

    public void AddWeapon()
    {
        hud.Refresh(this);
        hud.SetSelector(false);
    }

    public void StopCycling()
    {
        timer = 0;
        cycling = false;
        cycleIndex = activeIndex;
        hud.SetSelector(false);
    }

    public void SwitchWeapon(int index)
    {
        activeIndex = index;
        timer = 0;
        hud.SetSelector(false);
    }

    public void LoadWeaponFromName(string name)
    {
        Resources.Load(weaponResourcesPath + name);
    }

    public void RefillRandom(int amount)
    {
        // get all eligible weapons
        List<RangedWeapon> eligible = new();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].GetComponent<RangedWeapon>() == null) continue;
            RangedWeapon candidate = weapons[i].GetComponent<RangedWeapon>();

            if (candidate.GetMissingAmmo() > 0) eligible.Add(candidate);
        }

        if (eligible.Count == 0)
        {
            Debug.LogWarning("No eligible weapons found! In such case refrain from calling this method");
            return;
        }

        int chosen = Random.Range(0, weapons.Count);

        hud.NewAmmo(chosen, weapons[chosen].GetComponent<RangedWeapon>().GetAddableAmmo(amount));
        weapons[chosen].GetComponent<RangedWeapon>().AddAmmo(amount);
    }

    public void RefillSpecific(string identity, int amount)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            try
            {
                if (weapons[i].GetComponent<Weapon>().identity == identity)
                {
                    hud.NewAmmo(i, weapons[i].GetComponent<RangedWeapon>().GetAddableAmmo(amount));
                    weapons[i].GetComponent<RangedWeapon>().AddAmmo(amount);
                    break;
                }
            }
            catch { }
        }
    }

    public void RefillAll()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            try
            {
                RangedWeapon weapon = weapons[i].GetComponent<RangedWeapon>();

                hud.NewAmmo(i, weapon.GetMissingAmmo());
                weapon.AddMaxAmmo();
            }
            catch { /*In this case it was a melee weapon with no ammunition */ }
        }
    }
}