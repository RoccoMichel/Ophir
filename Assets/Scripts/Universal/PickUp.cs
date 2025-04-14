using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] Vector2 RandomBetween2Constants;
    public PickUpTypes type;
    public enum PickUpTypes { health, ammo }

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

                case PickUpTypes.ammo:

                    other.GetComponent<Inventory>().Refill(amount);
                    break;
            }
        }
    }
}
