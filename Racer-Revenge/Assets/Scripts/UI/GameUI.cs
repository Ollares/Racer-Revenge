using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
public class GameUI : MonoBehaviour
{
    private static GameUI _Instance;

    public static GameUI Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameUI>();
            }
            return _Instance;
        }
    }
    [Header("Canvas")]
    [SerializeField] Canvas _cameraCanvas;
    public Canvas CameraCanvas => _cameraCanvas;
    [Header("Screens")]
    public UIStartScreen StartScreen;
    public UIGameScreen GameScreen;
    public UIWinScreen WinScreen;
    public UILoseScreen LoseScreen;
    public UIUpgradeScreen UpgradeScreen;
    [Header("Panel")]
    public UILevelBar LevelBar;
    public UIResourceBar MoneyBar;
    public UIWorldFillBar ProgressBar;


    public void Initialize()
    {
        MoneyBar.UpdateAmount(GameData.Instance.Money);
        StartScreen.OnStart = () => { GameCore.Instance.StartLevel(); };
        WinScreen.OnNext = () => { GameCore.Instance.NextLevel(); };
        LoseScreen.OnRestart = () => { GameCore.Instance.RestartLevel(); };
    }

    public void ShowGameScreen()
    {
        ProgressBar.SetValue(0f);
        LevelBar.gameObject.SetActive(true);
        StartScreen.Close();
        WinScreen.Close();
        LoseScreen.Close();
        //UpgradeScreen.Close();
        GameScreen.Open();
    }

    public void ShowStartScreen()
    {
        LevelBar.gameObject.SetActive(true);
        StartScreen.Open();
        //UpgradeScreen.Open();
        WinScreen.Close();
        LoseScreen.Close();
        GameScreen.Close();
    }
    public void ShowWinScreen()
    {
        LevelBar.gameObject.SetActive(false);
        StartScreen.Close();
        WinScreen.Open();
        LoseScreen.Close();
        GameScreen.Close();
        //UpgradeScreen.Close();
    }
    public void ShowLoseScreen()
    {
        LevelBar.gameObject.SetActive(false);
        StartScreen.Close();
        WinScreen.Close();
        LoseScreen.Open();
        GameScreen.Close();
        //UpgradeScreen.Close();
    }
}
