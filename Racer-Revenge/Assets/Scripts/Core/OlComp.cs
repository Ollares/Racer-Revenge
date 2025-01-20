using UnityEngine;
public class OlComp : MonoBehaviour
{
    public static void Init()
    {
        
    }
    

    //USER DATA

    public static void SavePrefData(string key, int value)
    {
        SettingsUtils.SaveToPref(key, value);
    }

    public static string GetPrefStringData(string key)
    {
        return SettingsUtils.GetStringFromPrefs(key);
    }

    public static int GetPrefIntData(string key)
    {
        return SettingsUtils.GetIntFromPrefs(key);
    }

    public static void SaveUserData<T>(string key, T value) where T : SecureData
    {
        SettingsUtils.Save(key, value);
    }

    public static T GetUserData<T>(string key) where T : SecureData
    {
        return SettingsUtils.Load<T>(key);
    }


    public static void LevelStarted(int levelNumber, int attempt)
    {
       
    }
    public static void LevelFailed()
    {
        
    }
    public static void LevelCompleted(int levelNumber)
    {
        
    }

}
