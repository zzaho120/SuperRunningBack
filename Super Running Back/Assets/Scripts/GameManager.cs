using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputManager inputManager;
    public CameraManager mainCamera;
    public PlayerController player;
    public UIManager UI;
    public EnemyController[] enemys;
    public ScoreManager scoreManager;
    public PlayableDirector startGameTimeLine;
    public PlayableDirector weakTouchdownTimeLine;
    public CinemachineBrain cinemachineBrain;
    public RandomGenerateStage randomGenerateStage;
    public StartSetting startSetting;

    public GameState state;
    public int playTime;

    private float gameStartTime;
    private int totalScore;
    private bool isTutorial = true;

    private void Awake()
    {
        Instance = this;
        inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        UI = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
        startGameTimeLine = GameObject.FindWithTag("StartGameTimeLine").GetComponent<PlayableDirector>();
        randomGenerateStage = GetComponent<RandomGenerateStage>();
        //randomGenerateStage.Generate();
        
        enemys = GameObject.FindWithTag("Enemys").GetComponentsInChildren<EnemyController>();
        startSetting = GetComponent<StartSetting>();
        startSetting.GameStartInit(randomGenerateStage.stageInfo.yard);

        if (state != GameState.Game)
            inputManager.enabled = false;
    }

    private void Start()
    {
        player.Init();
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
                advertise();
                break;
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
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;
        ui.StopIncreasceScore();

        scoreManager.SetFinishTime(playTime);

        var yard = (randomGenerateStage.stageInfo.yard * 0.1f) - 5;
        //startSetting.PlayerResultSetting();
        UI.Open(UIs.Result);
    }

    public void PlayerDieMsg()
    {
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;
        ui.StopIncreasceScore();


        UI.Open(UIs.Gameover);
        player.PlayerDead();
        player.enabled = false;
        inputManager.enabled = false;
    }

    public void PlayerLevelUpMsg()
    {
        player.AnimationSpeedUp();
        player.SizeSetting();
        player.SetLevelUpEffect();

        var level = player.stats.currentLevel.level;
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;

        mainCamera.SetPlayerLevel(level);
        scoreManager.SetPlayerLevel(level);
        ui.SetSizeStatusBar();
    }

    public void GameStart()
    {
        state = GameState.Game;
        
        UI.Open(UIs.Game);

        startGameTimeLine.gameObject.SetActive(false);
        cinemachineBrain.enabled = false;
        
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
        DataManager.SaveCareer(scoreManager.finishTime, scoreManager.itemNumber,
            scoreManager.holdEnemyNumber, scoreManager.kickEnemyNumber, scoreManager.totalScore);

        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCount > sceneIndex)
        {
            DataManager.CurrentStageIndex = sceneIndex;
            Loader.Load(sceneIndex + 1);
        }
    }

    public void ReStart()
    {
        Loader.Load(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayStartGameTimeLine()
    {
        startGameTimeLine.Play();
        UI.Close();
    }

    public void PlayWeakTouchdownTimeLine()
    {
        cinemachineBrain.enabled = true;
        weakTouchdownTimeLine.gameObject.SetActive(true);
        weakTouchdownTimeLine.Play();
        UI.Close();
    }

    public void InplayPrintScore()
    {
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;
        ui.IncreaseScore();
    }

    public void GetItemMsg()
    {
        player.stats.currentItemCnt++;
        player.stats.CheckItem();
        scoreManager.AddItemNumber();
        InplayPrintScore();
    }

    private void advertise()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayStartGameTimeLine();
        }
    }
}