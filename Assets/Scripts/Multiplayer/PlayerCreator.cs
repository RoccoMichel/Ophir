using UnityEngine;
using Photon.Pun;

public class PlayerCreator : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected GameDirector director;
    public string[] charactersNames;
    public GameObject characterSelector;

    public void CreatePlayer(int index)
    {
        Camera.main.gameObject.SetActive(false);
        characterSelector.SetActive(false);
        PhotonNetwork.Instantiate(charactersNames[index], director.worldSpawnPoints[Random.Range(0, director.worldSpawnPoints.Length)], Quaternion.identity);
    }
}