using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject featureObject;
    [SerializeField] GameObject optionsObject;

    private void Start()
    {
        ToggleAllElements(false);
    }

    public void FeatureMenu()
    {
        optionsObject.SetActive(false);
        featureObject.SetActive(!featureObject.activeSelf);
    }
    public void OptionsMenu()
    {
        featureObject.SetActive(false);
        optionsObject.SetActive(!optionsObject.activeSelf);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

    void ToggleAllElements(bool b)
    {
        featureObject.SetActive(b);
        optionsObject.SetActive(b);
    }
}
