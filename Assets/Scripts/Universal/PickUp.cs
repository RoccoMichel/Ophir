using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Use if random amount allowed")]
    [SerializeField] Vector2 RandomBetween2Constants;
    public PickUpTypes type;
    public enum PickUpTypes 
    { 
        health, 
        armor, 
        ammoRandom, 
        ammoSpecific, 
        ammoAll,
        addWeapon,
        addAndEquipWeapon 
    }

    [Header("Ignore the following if not relevant")]
    public string specifier = "Leave be if not needed";
    public GameObject[] weapons;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int amount = (int)Random.Range(RandomBetween2Constants.x, RandomBetween2Constants.y);

            switch (type)
            {
                case PickUpTypes.health:

                    other.GetComponent<BasePlayer>().Heal(amount);
                    break;

                case PickUpTypes.armor:

                    other.GetComponent<BasePlayer>().AddArmor(amount);
                    break;

                case PickUpTypes.ammoRandom:

                    if (!other.GetComponent<Inventory>().TryRefillRandom(amount)) return;
                    break;

                case PickUpTypes.ammoSpecific:

                    if (!other.GetComponent<Inventory>().TryRefillSpecific(specifier, amount)) return;
                    break;

                case PickUpTypes.ammoAll:

                    if (!other.GetComponent<Inventory>().TryRefillAll()) return;
                    break;

                case PickUpTypes.addWeapon:

                    foreach (var weapon in weapons)
                        other.GetComponent<Inventory>().AddWeapon(weapon);
                    break;

                case PickUpTypes.addAndEquipWeapon:

                    foreach (var weapon in weapons)
                        other.GetComponent<Inventory>().AddAndEquipWeapon(weapon);
                    break;
            }

            Destroy(gameObject);
        }
    }

    // warn unknowing dev
    private void OnValidate()
    {
        foreach (Collider collider in GetComponents<Collider>())
            if (collider.isTrigger) return;
        
        Debug.LogWarning("Pick up needs to have a trigger!");
    }
}
