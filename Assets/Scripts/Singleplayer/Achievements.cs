using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Achievements are stored in a single string carrying 1s and or 0s
/// 1 = unlocked, 0 = locked. Using Unity's PlayerPrefs
/// Get/Set status per index for your own achievement system.
/// </summary>
public class Achievements : MonoBehaviour
{
    public static string key = "achievements";

    public static void ResetAchievements()
    {
        char[] achievementArray = PlayerPrefs.GetString(key, "0").ToCharArray();
        string resetString = string.Empty;

        for (int i = 0; i < achievementArray.Length; i++)
            resetString += "0";        

        PlayerPrefs.SetString(key, resetString);
    }

    public static void SetAchievementsCount(int count)
    {
        char[] achievementArray = PlayerPrefs.GetString(key, "0").ToCharArray();
        string newAchievementArray = string.Empty;

        for (int i = 0; i < count; i++)
        {
            if (i > achievementArray.Length) newAchievementArray += "0";
            else newAchievementArray += achievementArray[i];
        }
    }

    public static bool GetAchievementStatus(int index)
    {
        char[] achievementArray = PlayerPrefs.GetString(key, "0").ToCharArray();

        if (achievementArray[index] == '1') return true;
        else return false;
    }

    public static void SetAchievementStatus(int index, bool status)
    {
        char[] achievementArray = PlayerPrefs.GetString(key, "0").ToCharArray();

        if (index > achievementArray.Length)
        {
            Debug.LogError($"index {index} is beyond the achievement Array.");
            return;
        }

        achievementArray[index] = status.ConvertTo<char>();
        PlayerPrefs.SetString(key, achievementArray.ToString());
    }

    public static int GetAchievementsCount()
    {
        char[] achievementArray = PlayerPrefs.GetString(key, "0").ToCharArray();
        return achievementArray.Length;
    }

    /// <summary>
    /// Set all Achievements from a string. Useful when loading from cloud save or similar.
    /// </summary>
    /// <param name="newArray">String of 1s & 0s</param>
    public static void OverwriteAchievements(string newArray)
    {
        PlayerPrefs.GetString(key, newArray).ToCharArray();
    }
}