using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData : MonoBehaviour
{
    private static GameData _Instance;

    public static GameData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameData>();
            }
            return _Instance;
        }
    }

    [Header("User Data")]
    public bool ClearUserData = false;
    UserData userData = new UserData();
    int USER_DATA_VERSION = 0;

    public int Level { get { return userData.level; } set { userData.level = value; SaveUserData(); } }
    public int LevelPrefab { get { return userData.levelPrefab; } set { userData.levelPrefab = value; SaveUserData(); } }
    public int Zone { get { return userData.zone; } set { userData.zone = value; SaveUserData(); } }
    public int BestScore { get { return userData.bestScore; } set { userData.bestScore = value; SaveUserData(); } }
    public int Score { get; set; } = 0;
    public int Attempt { get { return userData.attempt; } set { userData.attempt = value; SaveUserData(); } }
    public bool Sound { get { return userData.sound; } set { userData.sound = value; SaveUserData(); } }
    public bool Music { get { return userData.music; } set { userData.music = value; SaveUserData(); } }
    public float SoundVolume { get { return userData.soundVolume; } set { userData.soundVolume = value; SaveUserData(); } }
    public float MusicVolume { get { return userData.musicVolume; } set { userData.musicVolume = value; SaveUserData(); } }
    public bool Vibration { get { return userData.vibration; } set { userData.vibration = value; SaveUserData(); } }
    public int Money { get { return userData.money; } set { userData.money = value; SaveUserData(); } }
    public LevelsDataSO levelsData;
    public EnemyData EnemyData;
    public ProjectilesDataSO ProjectilesData;
    public VFXData VFXData;
    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        if(ClearUserData == true)
        {
            userData = new UserData();
            OlComp.SaveUserData(UserData.USER_DATA_KEY, userData);
        }
        OlComp.Init();
        LoadUserData();
    }

    void LoadUserData()
    {
        var tmpUserData = OlComp.GetUserData<UserData>(UserData.USER_DATA_KEY);
        if (tmpUserData != null)
        {
            Level = tmpUserData.level;
            LevelPrefab = tmpUserData.levelPrefab;
            Zone = tmpUserData.zone;
            BestScore = tmpUserData.bestScore;
            Attempt = tmpUserData.attempt;
            Sound = tmpUserData.sound;
            Music = tmpUserData.music;
            SoundVolume = tmpUserData.soundVolume;
            MusicVolume = tmpUserData.musicVolume;
            Vibration = tmpUserData.vibration;
            Money = tmpUserData.money;
            int dataVersion = tmpUserData.dataVersion;
            if (dataVersion != 0)
            {
  
            }
        }
        else
        {
            SaveUserData();
            LoadUserData();
        }
    }

    public void SaveUserData()
    {
        userData.dataVersion = USER_DATA_VERSION;
        OlComp.SaveUserData(UserData.USER_DATA_KEY, userData);
    }

}



