using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] singleplayerElements;
    [SerializeField] GameObject[] multiplayerElements;
    [SerializeField] GameObject[] OptionElements;

    private void Start()
    {
        ToggleAllElements(false);
    }

    public void SingleplayerMenu()
    {
        ToggleAllElements(false);
        foreach (GameObject element in singleplayerElements) element.SetActive(true);
    }
    public void MultiplayerMenu()
    {
        ToggleAllElements(false);
        foreach (GameObject element in multiplayerElements) element.SetActive(true);
    }
    public void OptionsMenu()
    {
        ToggleAllElements(false);
        foreach (GameObject element in OptionElements) element.SetActive(true);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

    void ToggleAllElements(bool b)
    {
        foreach (GameObject element in singleplayerElements) element.SetActive(b);
        foreach (GameObject element in multiplayerElements) element.SetActive(b);
        foreach (GameObject element in OptionElements) element.SetActive(b);
    }
}
