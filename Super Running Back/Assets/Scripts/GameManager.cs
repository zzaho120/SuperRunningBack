using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputManager inputManager;
    public CameraManager mainCamera;
    public PlayerController player;
    public UIManager UI;
    public EnemyController[] enemys;
    public ScoreManager scoreManager;
    
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
        gameStartTime = Time.time;
    }

    private void Update()
    {
        playTime = (int)(Time.time - gameStartTime);

        if(isTutorial && playTime > 2f)
        {
            isTutorial = false;
            var gameUI = UI.GetComponentInChildren<GameUIContorller>();
            gameUI.TutorialBarOff();
        }
    }

    public void Finish()
    {
        scoreManager.SetFinishTime(playTime);
        UI.Open(UIs.Result);
    }

    public void PlayerDie()
    {
        //Time.timeScale = 0f;
        //PrintScore();
        UI.Open(UIs.Gameover);
        player.PlayerDead();
        player.enabled = false;
    }

    public void PlayerLevelUp()
    {
        player.AnimationSpeedUp();
        player.SizeSetting();
        mainCamera.GetPlayerLevel();
    }
}
