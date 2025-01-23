using UnityEngine;
using Photon.Pun;

public class PlayerCreator : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(CreatePlayer), 0.5f);
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate("MP_Player", transform.position, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
}