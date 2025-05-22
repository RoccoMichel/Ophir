using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    /// Ammo Player is carrying (active ammo excluded)
    /// </summary>
    public int carryingAmmo = 50;
    /// <summary>
    /// Max ammo player can hold for this weapon (active ammo excluded)
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
    /// <summary>
    /// Determains if the weapon can be shot
    /// </summary>
    public bool canFire = true;

    // Reload Variables
    /// <summary>
    /// True if the entire mag reloads at once, false if per shell
    /// </summary>
    public bool reloadAllAtOnce = true; 
    
    public ReloadType reload;
    public Transform barrel;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem[] muzzleFlash;
    [SerializeField] protected AudioClip shootSound; 
    [SerializeField] protected AudioClip reloadSound;
    private Animator animator;
    private AudioSource audioSource;

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

        canFire = true;
        reloadAction = InputSystem.actions.FindAction("Reload");
        TryGetComponent(out animator);
        TryGetComponent(out audioSource);

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

        if (reloadAction.WasPressedThisFrame()) StartCoroutine(Reload());

        if (automatic && attackAction.IsPressed() && timeSinceLastShot > 60 / rmp) // Automatic Firing
        {
            if (activeAmmo > 0) Shoot();
            else if (activeAmmo <= 0 && reload == ReloadType.FireReload) StartCoroutine(Reload());
        }

        else if (!automatic && attackAction.WasPressedThisFrame() && timeSinceLastShot > 60 / rmp) // Semi-Auto Firing
        {
            if (activeAmmo > 0) Shoot();
            else if (activeAmmo <= 0 && reload == ReloadType.FireReload) StartCoroutine(Reload());
        }
    }

    public virtual void Shoot()
    {
        if (!canFire) return;

        timeSinceLastShot = 0f;
        activeAmmo--;
        cam.FovKickback(fovKickback);

        foreach (ParticleSystem effect in muzzleFlash) effect.Play();
        if (animator != null) animator.Play("Fire");
        if (audioSource != null)
        {
            audioSource.clip = shootSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        if (activeAmmo <= 0 && reload == ReloadType.Automatic) StartCoroutine(Reload());
    }

    public virtual IEnumerator Reload()
    {
        if (activeAmmo >= capacityAmmo || carryingAmmo <= 0) yield break;
        if (!saveAmmo) activeAmmo = 0;

        float timer = 0;

        // Reload Instant
        if (reloadAllAtOnce && saveAmmo)
        {
            if (animator != null) animator.Play("Reload");
            if (audioSource != null)
            {
                audioSource.clip = reloadSound;
                audioSource.loop = false;
                audioSource.Play();
            }

            while (timer < animator.GetCurrentAnimatorClipInfo(0).Length)
            {
                Debug.Log(animator.GetCurrentAnimatorClipInfo(0).Length);

                // Reload was cancled
                if (attackAction.WasPressedThisFrame() && activeAmmo != 0) yield break;

                timer += Time.deltaTime;

                yield return null;
            }

            carryingAmmo += activeAmmo;
            activeAmmo = 0;

        }

        // Reload Induvidually
        while (activeAmmo < capacityAmmo)
        {
            if (carryingAmmo <= 0) yield break;

            if (animator != null) animator.Play("Reload");
            if (audioSource != null)
            {
                audioSource.clip = reloadSound;
                audioSource.loop = false;
                audioSource.Play();
            }

            while (timer < animator.GetCurrentAnimatorClipInfo(0).Length)
            {
                // Reload was cancled
                if (attackAction.WasPressedThisFrame() && activeAmmo != 0) yield break;

                timer += Time.deltaTime;

                yield return null;
            }

            activeAmmo++;
            carryingAmmo--;
            timer = 0;
        }
    }

    public virtual void AddAmmo(int amount)
    {
        carryingAmmo = Mathf.Clamp(carryingAmmo + amount, 0, maxAmmo);
    }
    public virtual void AddMaxAmmo()
    {
        carryingAmmo = maxAmmo;
    }
    public int GetMissingAmmo()
    {
        return maxAmmo - carryingAmmo;
    }
    /// <summary>
    /// Returns how much of amount can be added to this weapon's ammo
    /// </summary>
    public int GetAddableAmmo(int amount)
    {
        int freeSpace = maxAmmo - carryingAmmo;

        if (freeSpace > amount) return amount;
        else return amount - (amount - freeSpace);
    }
}