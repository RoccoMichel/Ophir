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
        }
        if (hud != null) hud.Refresh(this);
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

    public void AddWeapon(GameObject newWeapon)
    {
        weapons.Add(newWeapon);

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
        Weapon w = weapons[index].GetComponent<Weapon>();
        activeIndex = index;

        timer = 0;
        hud.SetSelector(false);
        hud.SetCrosshair(w.crosshair);

        if (weapons[activeIndex].gameObject.TryGetComponent(out RangedWeapon r)) hud.gunReference = r;
        else hud.gunReference = null;
    }

    public void LoadWeaponFromName(string name)
    {
        Resources.Load(weaponResourcesPath + name);
    }

    public void RefillRandom(int amount)
    {
        // get all eligible weapons
        List<RangedWeapon> eligible = new();
        List<int> indexes = new();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (!weapons[i].TryGetComponent(out RangedWeapon r)) continue;

            if (r.GetMissingAmmo() > 0)
            {
                eligible.Add(r);
                indexes.Add(i);
            }
        }

        if (eligible.Count == 0) // Exception
        {
            Debug.LogWarning("No eligible weapons found! In such case refrain from calling this method");
            return;
        }

        // choose random
        int chosen = Random.Range(0, eligible.Count);

        // give ammunition
        hud.NewAmmo(indexes[chosen], eligible[chosen].GetComponent<RangedWeapon>().GetAddableAmmo(amount));
        eligible[chosen].AddAmmo(amount);
    }

    public void RefillSpecific(string identity, int amount)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].GetComponent<Weapon>().identity == identity && weapons[i].TryGetComponent(out RangedWeapon r))
            {
                hud.NewAmmo(i, r.GetAddableAmmo(amount));
                r.AddAmmo(amount);
                break;
            }
        }
    }

    public void RefillAll()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].TryGetComponent(out RangedWeapon r)) // check if weapon has ammonition
            {
                hud.NewAmmo(i, r.GetMissingAmmo());
                r.AddMaxAmmo();
            }
        }
    }
}