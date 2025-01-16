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
        PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
    }
}