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
    public ScoreManager scoreManager;
    public PlayableDirector startGameTimeLine;
    public CinemachineBrain cinemachineBrain;
    
    public RandomGenerateStage randomGenerateStage;
    public StartSetting startSetting;

    public List<PlayableDirector> touchdowns;
    public EnemyController[] enemys;
    public FixedEnemyController[] fixedEnemys;
    public IncreaseStatsByCollision[] items;
    public YardText yardText;

    public GameState state;
    public int playTime;

    private float gameStartTime;
    private bool isTutorial = true;
    private bool isTouchdown;

    public GameObject[] touchdownPrefab;
    public GameObject[] touchdownPosition;

    private void Awake()
    {
        Instance = this;
        
        if (state != GameState.Game)
            inputManager.enabled = false;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        randomGenerateStage.Generate();
        player.Init();
        startSetting.GameStartInit(randomGenerateStage.currentStageInfo.yard);
        startSetting.Init();
        scoreManager.Init();
        yardText.Init();

        enemys = GameObject.FindWithTag("Enemys").GetComponentsInChildren<EnemyController>();
        fixedEnemys = GameObject.FindWithTag("FixedEnemys").GetComponentsInChildren<FixedEnemyController>();
        items = GameObject.FindWithTag("Items").GetComponentsInChildren<IncreaseStatsByCollision>();

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
        UI.Open(UIs.Result);

        for(int idx = 0; idx < touchdownPosition.Length; idx++)
        {
            Destroy(touchdownPosition[idx].transform.GetChild(0).gameObject);
        }
        foreach (var enemy in enemys)
        {
            ObjectPool.ReturnObject(PoolName.Enemy, enemy.gameObject);
        }
        foreach (var fixedEnemy in fixedEnemys)
        {
            ObjectPool.ReturnObject(PoolName.FixedEnemy, fixedEnemy.gameObject);
        }
        foreach (var item in items)
        {
            ObjectPool.ReturnObject(PoolName.Item, item.gameObject);
        }
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

        Debug.Log(enemys.Length);
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
            if(gameUI != null)
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

        DataManager.NextStage();
        for (int idx = 0; idx < touchdownPosition.Length; idx++)
        {
            Instantiate(touchdownPrefab[idx], touchdownPosition[idx].transform);
        }

        state = GameState.MainMenu;
        UI.Open(UIs.MainMenu);
        isTouchdown = false;

        startGameTimeLine.gameObject.SetActive(true);
        foreach (var touchdown in touchdowns)
        {
            touchdown.gameObject.SetActive(false);
        }

        foreach (var enemy in enemys)
        {
            ObjectPool.ReturnObject(PoolName.Enemy, enemy.gameObject);
        }
        foreach (var fixedEnemy in fixedEnemys)
        {
            ObjectPool.ReturnObject(PoolName.FixedEnemy, fixedEnemy.gameObject);
        }
        foreach (var item in items)
        {
            ObjectPool.ReturnObject(PoolName.Item, item.gameObject);
        }

        Init();
    }

    public void ReStart()
    {
        state = GameState.MainMenu;
        UI.Open(UIs.MainMenu);

        startGameTimeLine.gameObject.SetActive(true);
        cinemachineBrain.enabled = true;

        player.enabled = true;

        
        Init();
    }

    public void PlayStartGameTimeLine()
    {
        UI.Close();
        startGameTimeLine.Play();
    }

    public void PlayTouchdown()
    {
        if(!isTouchdown)
        {
            isTouchdown = true;

            UI.Close();
            cinemachineBrain.enabled = true;

            scoreManager.SetFinishTime(playTime);
            scoreManager.SetTotalScore();

            var totalScore = scoreManager.GetTotalScore();
            var yard = randomGenerateStage.currentStageInfo.yard;
            PlayTouchdownByScore(totalScore);
        }
    }

    public void InplayPrintScore()
    {
        if (!isTouchdown)
        {
            var ui = UI.GetUI(UIs.Game) as GameUIContorller;
            ui.IncreaseScore();
        }
    }

    public void GetItemMsg()
    {
        player.stats.currentItemCnt++;
        player.stats.CheckItem();
        scoreManager.AddItemNumber();

        var soundObj = ObjectPool.GetObject(PoolName.ItemSound);
        var sound = soundObj.GetComponent<AudioSource>();
        sound.Play();
        InplayPrintScore();
    }
    
    private void PlayTouchdownByScore(int score)
    {
        PlayableDirector touchdown;
        if(score < 15000)
        {
            touchdown = touchdowns[(int)Touchdown.Weak];
        }
        else if(score < 20000)
        {
            touchdown = touchdowns[(int)Touchdown.Middle];
        }
        else
        {
            touchdown = touchdowns[(int)Touchdown.Strong];
        }
        touchdown.gameObject.SetActive(true);
        touchdown.Play();
    }
}