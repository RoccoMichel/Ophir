using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField nameField;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        print("Connecting...");
    }

    public override void OnConnectedToMaster()
    {
        print($"Connected! Server:{PhotonNetwork.CloudRegion}");
        startButton.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeName()
    {
        PhotonNetwork.NickName = nameField.text;
        print($"Changed Name to: {nameField.text}");
    }
}
