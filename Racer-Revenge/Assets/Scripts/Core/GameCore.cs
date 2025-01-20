using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;
using UnityEngine.Events;


public enum GameState
{
    Start, Play, Win, Lose
}
public class GameCore : MonoBehaviour
{
    private static GameCore _Instance;

    public static GameCore Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameCore>();
            }
            return _Instance;
        }
    }
    public CinemachineGameCamera cinemachineGameCamera;

    [SerializeField] PlayerController playerControllerPrefab;
    [SerializeField] SlideInputs slideInputsPrefab;
    [HideInInspector] public PlayerController PlayerController;
    [HideInInspector] public SlideInputs SlideInputs;
    [HideInInspector] public Level Level;
    private bool isLevelLoading = false;

    public static int LayerGround = 1 << 9;
    public static int LayerPlayer = 1 << 15;
    public static int LayerUnit = 1 << 19;
    public static int LayerEnemy = 1 << 16;
    void Awake()
    {
        StartGame();
    }
    [HideInInspector] public GameState currentState;
    public void SetStateGame(GameState state)
    {
        currentState = state;
        switch (currentState)
        {
            case GameState.Start:
                GameUI.Instance.ShowStartScreen();
                break;
            case GameState.Play:
                GameUI.Instance.ShowGameScreen();
                SlideInputs.EnableInputs(true);
                PlayerController.carController.StartController();
                break;
            case GameState.Lose:
                GameUI.Instance.ShowLoseScreen();
                SlideInputs.EnableInputs(false);
                LevelLose();
                break;
            case GameState.Win:
                GameUI.Instance.ShowWinScreen();
                SlideInputs.EnableInputs(false);
                LevelWin();
                break;
        }
    }

    public void StartGame()
    {
        GameUI.Instance.Initialize();
        LoadLevel();
        SetStateGame(GameState.Start);
    }
    public void LevelWin()
    {
        HapticManager.HapticLevelPassed();
        GameData.Instance.Level++;
        GameData.Instance.LevelPrefab++;
    }

    public void LevelLose()
    {
        HapticManager.HapticGameOver();
    }

    public void LoadLevel()
    {
        if (isLevelLoading == false)
        {
            StartCoroutine(LoadLevelCoroutine());
        }
    }
    public void StartLevel()
    {
        SetStateGame(GameState.Play);
    }

    public void NextLevel()
    {
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }

        LoadLevel();
        SetStateGame(GameState.Start);
        HapticManager.HapticLevelPassed();
    }

    public void RestartLevel()
    {
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }

        LoadLevel();
        SetStateGame(GameState.Start);
        HapticManager.HapticLevelPassed();
    }

    IEnumerator LoadLevelCoroutine()
    {
        isLevelLoading = true;

        PoolController.Instance.ResetPools();
        PoolController.Instance.Initialize();
        
        int amount = GameData.Instance.levelsData.Levels.Count;
        int levelNumber = GameData.Instance.Level;
        

        if (GameData.Instance.LevelPrefab >= amount)
                GameData.Instance.LevelPrefab = 0;
        
            int nextLevelNumber = GameData.Instance.LevelPrefab;
            var nextLevel = GameData.Instance.levelsData.Levels[nextLevelNumber];
            
            Level = Instantiate(nextLevel).GetComponent<Level>();
            Level.transform.position = Vector3.zero;
            Level.Initialize();
            GameUI.Instance.LevelBar.UpdateLevelText("Lvl." + levelNumber);

            InitializePlayerController();
            InitializeInputController();
        yield return null;
        isLevelLoading = false;
    }

    public void InitializePlayerController()
    {
        if(PlayerController != null)
            Destroy(PlayerController.gameObject);
        PlayerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
    }
    
    public void InitializeInputController()
    {
        if(SlideInputs != null)
            Destroy(SlideInputs.gameObject);
        SlideInputs = Instantiate(slideInputsPrefab, Vector3.zero, Quaternion.identity);
    }
}

