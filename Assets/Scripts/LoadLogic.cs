using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLogic : MonoBehaviour
{
    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadNextInBuild()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static void LoadPreviousInBuild()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public static void LoadSceneByNumber(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void LoadSceneByPlayerPrefInt(string tag)
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(tag));
    }
    public static void LoadSceneByPlayerPrefString(string tag)
    {
        SceneManager.LoadScene(PlayerPrefs.GetString(tag));
    }
    public static void QuitGame()
    {
        Application.Quit();
        Debug.LogWarning("Player Quit");
    }
    public static void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}