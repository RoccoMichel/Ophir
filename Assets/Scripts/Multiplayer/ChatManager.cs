using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public List<GameObject> messagesList = new();
    [SerializeField] GameObject messageReference;

    private void Update()
    {
        while (messagesList.Count > 9)
        {
            GameObject msg = messagesList[0];
            Destroy(msg);
            messagesList.RemoveAt(0);
        }

        for (int i = 0; i < messagesList.Count; i++)
        {
            if (messagesList[i] == null)
            {
                messagesList.RemoveAt(i);
                i--;
                continue;
            }

            messagesList[i].transform.position = new Vector3(20, 130 + (100 * messagesList.Count-i));
        }
    }

    public void ShowMessage(string username, string message)
    {
        if (string.IsNullOrEmpty(username)) username = "Anonyms";

        GameObject msg = Instantiate(messageReference, FindAnyObjectByType<Canvas>().transform);

        messagesList.Add(msg);

        TMP_Text[] uiElement = msg.GetComponentsInChildren<TMP_Text>();
        uiElement[0].text = message;
        uiElement[1].text = username;
    }
}
