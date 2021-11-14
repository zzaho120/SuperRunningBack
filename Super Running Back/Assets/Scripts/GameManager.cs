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
    public LinkedList<GameObject> enemys = new LinkedList<GameObject>();
    public LinkedList<GameObject> fixedEnemys = new LinkedList<GameObject>();
    public LinkedList<GameObject> items = new LinkedList<GameObject>();
    public YardText yardText;

    public GameState state;
    public int playTime;
    public bool isAdEnd;

    private float gameStartTime;
    private bool isTutorial = true;
    private bool isTouchdown;

    public GameObject touchdownPrefab;
    public GameObject touchdownPosition;

    private void Awake()
    {
        Instance = this;
        
        if (state != GameState.Game)
            inputManager.enabled = false;
    }

    private void Start()
    {
        DataManager.LoadData();
        DataManager.LoadScene();
        AdManager.AdInit();
        Init();
    }

    private void Init()
    {
        state = GameState.MainMenu;
        UI.Open(UIs.MainMenu);

        ReturnListAllObject();
        randomGenerateStage.Generate();
        player.Init();
        startSetting.GameStartInit();
        startSetting.Init();
        scoreManager.Init();
        yardText.Init();
        ListInit();

        player.enabled = true;
        startGameTimeLine.gameObject.SetActive(true);
        cinemachineBrain.enabled = true;
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
                if (isAdEnd)
                {
                    isAdEnd = false;
                    if (DataManager.IsMaxStage)
                        GameManager.Instance.NextChapter();
                    else
                        GameManager.Instance.NextStage();
                }
                break;
            case GameState.Game:
                GameUpdate();
                break;
        }
    }

    public void ListInit()
    {
        var tempEnemys = GameObject.FindWithTag("Enemys").GetComponentsInChildren<EnemyController>();
        foreach (var enemy in tempEnemys)
        {
            enemys.AddLast(enemy.gameObject);
        }
        var tempFixedEnemys = GameObject.FindWithTag("FixedEnemys").GetComponentsInChildren<FixedEnemyController>();
        foreach (var fixedEnemy in tempFixedEnemys)
        {
            fixedEnemys.AddLast(fixedEnemy.gameObject);
        }
        var tempItems = GameObject.FindWithTag("Items").GetComponentsInChildren<ItemController>();
        foreach (var item in tempItems)
        {
            item.Init();
            items.AddLast(item.gameObject);
        }
    }

    public void Finish()
    {
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;
        ui.StopIncreasceScore();
        UI.Open(UIs.Result);

        ReturnListAllObject();
    }

    public void PlayerDieMsg()
    {
        var ui = UI.GetUI(UIs.Game) as GameUIContorller;
        ui.StopIncreasceScore();

        UI.Open(UIs.Gameover);
        state = GameState.Gameover;
        player.PlayerDead();
        player.enabled = false;
        inputManager.enabled = false;
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
            var enemy = elem.GetComponent<EnemyController>();
            enemy.GameStart();
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

    public void NextStage()
    {
        DataManager.SaveCareer(scoreManager.finishTime, scoreManager.itemNumber,
            scoreManager.holdEnemyWeight, scoreManager.kickEnemyNumber, scoreManager.totalScore);
        DataManager.NextStage();

        Destroy(touchdownPosition.transform.GetChild(0).gameObject);
        Instantiate(touchdownPrefab, touchdownPosition.transform);
        

        isTouchdown = false;
        foreach (var touchdown in touchdowns)
        {
            touchdown.gameObject.SetActive(false);
        }

        Init();
    }

    public void NextChapter()
    {
        DataManager.SaveCareer(scoreManager.finishTime, scoreManager.itemNumber,
            scoreManager.holdEnemyWeight, scoreManager.kickEnemyNumber, scoreManager.totalScore);

        DataManager.NextStage();
        var scene = SceneManager.GetActiveScene().buildIndex;
        Loader.Load(scene + 1);
    }

    public void ReStart()
    {
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
            state = GameState.Result;
            ReturnListAllObject();

            UI.Close();
            cinemachineBrain.enabled = true;

            scoreManager.SetFinishTime(playTime);
            scoreManager.SetTotalScore();

            var totalScore = scoreManager.GetTotalScore();
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

    public void MsgGetItem()
    {
        player.MsgGetItem();
        scoreManager.AddItemNumber();

        var soundObj = ObjectPool.GetObject(PoolName.ItemSound);
        var sound = soundObj.GetComponent<AudioSource>();
        sound.Play();
        InplayPrintScore();
    }
  
    private void PlayTouchdownByScore(int score)
    {
        PlayableDirector touchdown;
        if(score < 25000)
        {
            touchdown = touchdowns[(int)Touchdown.Weak];
        }
        else if(score < 30000)
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

    private void ReturnListAllObject()
    {
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
        enemys.Clear();
        fixedEnemys.Clear();
        items.Clear();
    }

    public void ReturnListObject(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Enemy":
                enemys.Remove(enemys.Find(obj));
                break;
            case "FixedEnemy":
                fixedEnemys.Remove(fixedEnemys.Find(obj));
                break;
            case "Item":
                items.Remove(items.Find(obj));
                break;
        }
    }

    public void PlayAd()
    {
        AdManager.OnClickInterstitial();
    }

    public void PlayRewardAd()
    {
        AdManager.OnClickReward();
    }
}