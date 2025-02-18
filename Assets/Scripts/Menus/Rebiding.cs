using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
    public InputActionAsset action;
    public GameObject rebindUI;
    public List<TMP_Text> movement;
    public TMP_Text jump;

    // public TMP_Text ExampleKey
    int id;
    List<string> movementBindingID = new();
    List<string> weaponsBindingID = new();
    public static bool isRebinding = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("RebindSuccess") == 1)
            action.LoadBindingOverridesFromJson(PlayerPrefs.GetString("rebinds"));

        UpdateKeyBinnedText();
    }
    public void SetId(int setId) { id = setId; }
    public void Rebind(string nameOfAction)
    {
        isRebinding = true;
        rebindUI.SetActive(true);

        InputAction actionToRebind = action.FindAction(nameOfAction);
        Debug.LogWarning("Started Rebinding on " + actionToRebind.bindings[id].path);
        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(id)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                isRebinding = false;
                PlayerPrefs.SetString("rebinds", action.SaveBindingOverridesAsJson());
                PlayerPrefs.SetInt("RebindSuccess", 1);
                rebindUI.SetActive(false);
                UpdateKeyBinnedText();
                actionToRebind.Enable();
                operation.Dispose();
            })
            .OnCancel(operation =>
            {
                isRebinding = false;
                rebindUI.SetActive(false);
                actionToRebind.Enable();
                operation.Dispose();
            });
            rebind.Start();
    }
    private void UpdateKeyBinnedText()
    {
        id = -1;
        movementBindingID = new List<string>();
        weaponsBindingID = new List<string>();

        InputAction movementAction = action.FindAction("Move");
        InputAction jumpAction = action.FindAction("Jump");
        InputAction weapons = action.FindAction("Switch");
        // InputAction example = action.FindAction("Example");

        for (int i = 0; i < movementAction.bindings.Count; i++) 
            movementBindingID.Add(InputControlPath.ToHumanReadableString(movementAction.bindings[i].effectivePath, 
                InputControlPath.HumanReadableStringOptions.OmitDevice));
        for (int i = 0; i < movement.Count; i++) 
            movement[i].text = movementBindingID[i + 1];
        
        jump.text = InputControlPath.ToHumanReadableString(jumpAction.bindings[0].effectivePath, 
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        for (int i = 0; i < weapons.bindings.Count; i++) 
                weaponsBindingID.Add(InputControlPath.ToHumanReadableString(weapons.bindings[i].effectivePath, 
                    InputControlPath.HumanReadableStringOptions.OmitDevice));

        // ExampleKey.text = weaponsBinding[index];
    }
}