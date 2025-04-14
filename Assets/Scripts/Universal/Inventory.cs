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

        // DEBUGGING ///////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.F))
        {
            //hud.Refresh(this);
        }
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

    public void Refill(int amount)
    {
        foreach (var weapon in weapons)
        {
            try { weapon.GetComponent<RangedWeapon>().AddAmmo(amount); }
            catch { }
        }
    }
}