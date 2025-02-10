using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Weapons that shoot something
/// </summary>
public class RangedWeapon : Weapon
{
    // Ammo Variables
    /// <summary>
    /// Rate of Fire per minute
    /// </summary>
    [Header("Gun Settings")]
    [Range(10, 600)]
    public int rmp = 60;
    /// <summary>
    /// Ammo in the weapons chamber
    /// </summary>
    public int activeAmmo = 10;
    /// <summary>
    /// Max ammo weapon can hold at a time
    /// </summary>
    public int capacityAmmo = 10;
    /// <summary>
    /// Ammo Player is carrying (ammo in weapons excluded)
    /// </summary>
    public int carryingAmmo = 50;
    /// <summary>
    /// Max ammo player can hold for this weapon (ammo in weapons excluded)
    /// </summary>
    public int maxAmmo = 100;
    /// <summary>
    /// False if activeAmmo gets thrown away, true to save it to carryingAmmo
    /// </summary>
    public bool saveAmmo = true;
    /// <summary>
    /// False means semi-auto, true means fully-auto
    /// </summary>
    public bool automatic = false;

    // Reload Variables
    /// <summary>
    /// True if the entire mag reloads at once, false if per shell
    /// </summary>
    public bool reloadAllAtOnce = true; 
    
    public ReloadType reload;
    public Transform barrel;

    // References
    public Image crosshair;
    protected InputAction reloadAction;
    protected Transform playerView;

    // variables for spread, kickback & charge to fire

    public enum ReloadType
    {
        /// <summary>
        /// Requires manual reload (press reload button)
        /// </summary>
        Manual,

        /// <summary>
        /// Reloads when fire button is pressed again and ammo is empty
        /// </summary>
        FireReload,

        /// <summary>
        /// Automatically reloads when ammo is empty
        /// </summary>
        Automatic,
    }

    public override void VariableAssignment()
    {
        base.VariableAssignment();
        reloadAction = InputSystem.actions.FindAction("Reload");
        if (barrel == null) barrel = gameObject.transform;

        if (playerView == null)
        {
            try { playerView = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform; }
            catch
            {
                if (Camera.main == null) playerView = transform;
                else playerView = Camera.main.transform;
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (reloadAction.WasPressedThisFrame()) Reload();

        if (automatic && attackAction.IsPressed() && timeSinceLastShot > 60 / rmp) // Automatic Firing
        {
            if (activeAmmo > 0) Shoot();
            else if (activeAmmo <= 0 && reload == ReloadType.FireReload) Reload();
        }

        else if (!automatic && attackAction.WasPressedThisFrame() && timeSinceLastShot > 60 / rmp) // Semi-Auto Firing
        {
            if (activeAmmo > 0) Shoot();
            else if (activeAmmo <= 0 && reload == ReloadType.FireReload) Reload();
        }
    }

    public virtual void Shoot()
    {
        timeSinceLastShot = 0f;

        activeAmmo--;
        if (activeAmmo <= 0 && reload == ReloadType.Automatic) Reload();
    }

    public virtual void Reload()
    {
        if (activeAmmo >= capacityAmmo) return;

        if (reloadAllAtOnce && saveAmmo)
        {
            carryingAmmo += activeAmmo;
            activeAmmo = 0;
        }
        else if (reloadAllAtOnce && !saveAmmo)
            activeAmmo = 0;

        while (activeAmmo < capacityAmmo)
        {
            if (carryingAmmo <= 0) return;

            activeAmmo++;
            carryingAmmo--;
        }
    }
}