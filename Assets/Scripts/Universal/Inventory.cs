using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static string weaponResourcesPath = "Singleplayer/Weapons/...";
    public bool canCycle = true;
    public int activeIndex;
    public int cycleIndex;
    public float cycleTime = 1f;
    public List<GameObject> weapons;

    internal bool cycling = false;
    internal InputAction cycleAction;
    internal InputAction confirmAction;

    private float timer = 0;

    private void Start()
    {
        cycleAction = InputSystem.actions.FindAction("Cycle");
        confirmAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (canCycle && cycleAction.WasPressedThisFrame())
        {
            cycleIndex += (int)cycleAction.ReadValue<float>();

            if (cycleIndex < 0) cycleIndex = weapons.Count - 1;
            if (cycleIndex > weapons.Count - 1) cycleIndex = 0;

            cycling = true;
            timer = cycleTime;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0) StopCycling();
        }

        if (cycling && confirmAction.WasPressedThisFrame())
            SwitchWeapon(cycleIndex);

    }

    public void StopCycling()
    {
        timer = 0;
        cycling = false;
        cycleIndex = activeIndex;
    }

    public void SwitchWeapon(int index)
    {
        activeIndex = index;
        timer = 0;
    }

    public void LoadWeaponFromName(string name)
    {
        Resources.Load(weaponResourcesPath + name);
    }
}