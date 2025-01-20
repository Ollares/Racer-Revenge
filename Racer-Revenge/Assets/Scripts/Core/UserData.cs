using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : SecureData
{
    public static string USER_DATA_KEY = "USER_DATA";

	public int level = 1;
	public int levelPrefab = 0;
	public int questionIndex = 0;
	public int questionIndexObstacle = 0;
	public int zone = 1;
	public int bestScore = 0;
	public int attempt = 0;
    public int money = 0;
    public bool sound = true;
	public bool music = true;
	public float soundVolume = 1;
	public float musicVolume = 1;
	public bool vibration = true;
}
