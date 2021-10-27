using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputManager inputManager;
    public CameraManager mainCamera;
    public PlayerController player;
    public UIManager UI;
    public EnemyController[] enemys;
    public ScoreManager scoreManager;
    public int yardInGround;
    public GameState state;


    private float gameStartTime;
    public int playTime;
    private int totalScore;
    private bool isTutorial = true;

    private void Awake()
    {
        Instance = this;
        inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        enemys = GameObject.FindWithTag("Enemys").GetComponentsInChildren<EnemyController>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
        UI = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
        
        inputManager.enabled = false;
    }

    private void Start()
    {
        var level = player.stats.currentLevel.level;

        mainCamera.SetPlayerLevel(level);
        scoreManager.SetPlayerLevel(level);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.None:
            case GameState.MainMenu:
            case GameState.Gameover:
            case GameState.Result:
                break;
            case GameState.Game:
                GameUpdate();
                break;
        }
        
    }

    public void Finish()
    {
        scoreManager.SetFinishTime(playTime);
        UI.Open(UIs.Result);
    }

    public void PlayerDie()
    {
        UI.Open(UIs.Gameover);
        player.PlayerDead();
        player.enabled = false;
        inputManager.enabled = false;
    }

    public void PlayerLevelUp()
    {
        player.AnimationSpeedUp();
        player.SizeSetting();

        var level = player.stats.currentLevel.level;

        mainCamera.SetPlayerLevel(level);
        scoreManager.SetPlayerLevel(level);
    }

    public void GameStart()
    {
        state = GameState.Game;
        UI.Open(UIs.Game);
        inputManager.enabled = true;
        player.GameStart();
        gameStartTime = Time.time;
        foreach (var elem in enemys)
        {
            elem.GameStart();
        }
    }

    public void GameUpdate()
    {
        playTime = (int)(Time.time - gameStartTime);

        if (isTutorial && playTime > 2f)
        {
            isTutorial = false;
            var gameUI = UI.GetComponentInChildren<GameUIContorller>();
            gameUI.TutorialBarOff();
        }
    }

    public void BackToMainMenu()
    {
        UI.Open(UIs.MainMenu);
    }

    public void OpenStageUI()
    {
        UI.Open(UIs.Stage);
    }

    public void OpenOptionUI()
    {
        UI.Open(UIs.Option);
    }
    public void OpenCareerUI()
    {
        UI.Open(UIs.Career);
    }

    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
