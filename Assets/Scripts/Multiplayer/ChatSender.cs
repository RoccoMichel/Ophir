using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatSender : MonoBehaviour
{
    [SerializeField] private ChatManager chatManager;
    [SerializeField] private TMP_InputField message;
    public void SendMessage()
    {
        // Don't send an empty message
        if (string.IsNullOrEmpty(message.text)) return;

        if (chatManager.enterChatAction.WasPressedThisFrame())
        {
            chatManager.GetComponent<PhotonView>().RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName, message.text);
            message.text = string.Empty;
        }
    }
}