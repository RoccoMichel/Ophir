using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public bool canCycle = true;
    public int activeIndex;
    public int cycleIndex;
    public float cycleTime = 1f;
    public List<GameObject> weapons;
    public PlayerHUD hud;
    public Transform handTransform;

    internal Weapon activeWeapon;
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

    // Switching Weapon Methods:
    public void AddAndEquipWeapon(GameObject weapon)
    {
        // Pick Up
        AddWeapon(weapon);

        // Equip
        SwitchWeapon(weapons.Count - 1);

        // Updating UI
        hud.Refresh(this);
        hud.SetSelector(false);
    }

    public void ParentToHand(Transform item)
    {
        item.transform.SetPositionAndRotation(handTransform.position, handTransform.rotation);
        item.parent = handTransform;
    }

    public void AddWeapon(GameObject newWeapon)
    {
        if (newWeapon == null) return;

        // Instantiate object if it isn't in the scene
        if (!newWeapon.scene.IsValid())
            newWeapon = Instantiate(newWeapon);

        // Assigning and setting the Object
        weapons.Add(newWeapon);
        ParentToHand(newWeapon.transform);
        newWeapon.SetActive(false);

        hud.Refresh(this);
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
        if (index < 0 || index >= weapons.Count) return;

        // Set variables
        GameObject newWeapon = weapons[index];
        Weapon w = newWeapon.GetComponent<Weapon>();
        activeIndex = index;

        // Switching Weapon
        try
        {
            activeWeapon.gameObject.SetActive(false);
        }
        catch
        {
            if (activeWeapon != null) Debug.LogWarning("'activeWeapon' could not be toggled!");
        }
        finally
        {
            // if weapons[index] is a out-of-scene prefab
            if (!newWeapon.scene.IsValid())
            {
                newWeapon = Instantiate(newWeapon);

                // Assigning and setting the Object
                weapons[index] = newWeapon;
                weapons.Add(newWeapon);                
            }

            // fix if newWeapon has incorrect parent
            if (newWeapon.transform.parent != handTransform) ParentToHand(newWeapon.transform);

            newWeapon.SetActive(true);
            activeWeapon = newWeapon.GetComponent<Weapon>();
        }

        // Update variables
        StopCycling();
        hud.SetCrosshair(w.crosshair);

        // Try setting optional weapons
        if (weapons[activeIndex].TryGetComponent(out RangedWeapon r)) hud.gunReference = r;
        else hud.gunReference = null;
    }

    // Refill Gun(s) Methods:
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

    public bool TryRefillRandom(int amount)
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

        if (eligible.Count == 0) // Exception of weapons that can get no new ammo
        {
            return false;
        }

        // choose random
        int chosen = Random.Range(0, eligible.Count);

        // give ammunition
        hud.NewAmmo(indexes[chosen], eligible[chosen].GetComponent<RangedWeapon>().GetAddableAmmo(amount));
        eligible[chosen].AddAmmo(amount);

        return true;
    }
    public bool TryRefillSpecific(string identity, int amount)
    {
        bool success = false;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].GetComponent<Weapon>().identity == identity && weapons[i].TryGetComponent(out RangedWeapon r))
            {
                if (r.GetMissingAmmo() > 0) success = true;

                hud.NewAmmo(i, r.GetAddableAmmo(amount));
                r.AddAmmo(amount);
                break;
            }
        }

        return success;
    }
    public bool TryRefillAll()
    {
        bool success = false;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].TryGetComponent(out RangedWeapon r)) // check if weapon has ammonition
            {
                if (r.GetMissingAmmo() > 0) success = true;
                hud.NewAmmo(i, r.GetMissingAmmo());
                r.AddMaxAmmo();
            }
        }

        return success;
    }
}