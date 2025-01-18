using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    [Header("Attributes")]
    public bool inChat;
    public int fontSize;
    public float chatLifeSeconds;

    [Header("References")]
    public TMP_Text recent;
    public TMP_Text full;
    public TMP_InputField chatInput;
    public Component[] fullChatFeatures;

    [HideInInspector] public List<string> messages = new();
    public List<float> timers = new();

    internal InputAction enterChatAction;
    internal InputAction cancelAction;

    private void Start()
    {
        enterChatAction = InputSystem.actions.FindAction("EnterChat");
        cancelAction = InputSystem.actions.FindAction("Cancel");

        inChat = false;
        ToggleActiveChat(false);
    }

    private void Update()
    {
        for (int i = 0;  i < timers.Count; i++)
        {
            timers[i] -= Time.deltaTime;

            if (timers[i] > 0) continue;

            timers.RemoveAt(i);
            messages.RemoveAt(i);
            RefreshRecentChat();
        }

        // Toggle which type of Chat is displayed
        if (enterChatAction.WasPressedThisFrame() || (inChat && cancelAction.WasPressedThisFrame()))
        {
            inChat = !inChat;
            ToggleActiveChat(inChat);

            if (inChat) chatInput.ActivateInputField();
        }
    }

    [PunRPC]
    public virtual void ShowMessage(string username, string message)
    {
        if (string.IsNullOrEmpty(username)) username = "ANONYMOUS";

        messages.Add($"[{username}] {message}");
        timers.Add(chatLifeSeconds);
        RefreshRecentChat();

        full.text = $"{full.text}[{username}] {message}\n\n";
    }

    public virtual void RefreshRecentChat()
    {
        recent.text = string.Empty;

        for (int i = 0; i < messages.Count; i++)
        {
            recent.text = $"{recent.text}{messages[i]}\n\n";
        }
    }

    /// <summary>
    /// Change which Chat is displayed to the Player by a boolean
    /// </summary>
    /// <param name="b">True indicates being in the full chat and False being in recent mode</param>
    public virtual void ToggleActiveChat(bool b)
    {
        full.fontSize = b ? fontSize : 0;
        recent.fontSize = b ? 0 : fontSize;

        foreach (Component component in fullChatFeatures)
        {
            if (component is Behaviour behaviour) behaviour.enabled = b;
        }
    }
}