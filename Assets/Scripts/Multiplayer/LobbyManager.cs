using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public void JoinGame()
    {
        PhotonNetwork.JoinRandomRoom();
        print("trying to join room...");

        // PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"couldn't join a room! {message}");
        CreateRoom();
    }

    void CreateRoom()
    {
        print("creating room...");

        float num = Random.Range(10000, 99999);
        RoomOptions options = new()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10,
        };

        PhotonNetwork.CreateRoom($"Lobby#{num}", options);
        print($"room {num} has been created!");
    }
}
