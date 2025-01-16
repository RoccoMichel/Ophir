using TMPro;
using UnityEngine;

public class ChatSender : MonoBehaviour
{
    [SerializeField] private ChatManager chatManager;
    [SerializeField] private TMP_InputField message;
    public void SendMessage()
    {
        chatManager.ShowMessage(Photon.Pun.PhotonNetwork.NickName, message.text);
        message.text = string.Empty;
    }
}
