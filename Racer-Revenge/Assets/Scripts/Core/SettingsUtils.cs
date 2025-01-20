using System.IO;
using UnityEngine;

public class SettingsUtils {

    private const string storageFolder = "/data";

    public static bool ToggleSound(AudioSource audioSource) {
        audioSource.mute = !audioSource.mute;
        var isMuted = audioSource.mute;
        return isMuted;
    }

    public static void EnableSound(AudioSource audioSource, bool enable) {
        audioSource.mute = !enable;
    }

    public static void SaveToPref(string key, string value) {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static void SaveToPref(string key, int value) {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static string GetStringFromPrefs(string key) {
        return PlayerPrefs.GetString(key, "");
    }

    public static int GetIntFromPrefs(string key) {
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void Save<T>(string dataKey, T data) where T: SecureData {
        string fileFolder = Application.persistentDataPath + storageFolder;
        if (!Directory.Exists(fileFolder)) {
            Directory.CreateDirectory(fileFolder);
        }
        string filePath = Path.Combine(fileFolder, dataKey + ".json");
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }

    public static T Load<T>(string dataKey) where T: SecureData {
        string filePath = Path.Combine(
            Application.persistentDataPath + storageFolder, dataKey + ".json");

        if (File.Exists(filePath)) {
            return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
        }
        return null;
    }

}
