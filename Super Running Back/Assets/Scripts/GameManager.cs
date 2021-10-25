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

    public void PrintScore()
    {
        if (!player.isDead)
        {
            totalScore += 5000;
        }

        // 나중에 게임매니저에 따로 저장할 공간이 있으면 좋을 듯
        var ragdoll = GameObject.FindGameObjectsWithTag("EnemyRagdoll");

        foreach(var elem in ragdoll)
        {
            var ragdollMgr = elem.GetComponent<RagdollManager>();
            totalScore += ragdollMgr.stats.level * 100;
            Debug.Log($"totalScore : {totalScore} += 붙잡은 적 레벨 {ragdollMgr.stats.level} * 100");
        }

        totalScore += player.stats.currentLevel.level * 1000;
        Debug.Log($"totalScore : {totalScore} += 플레이어 레벨 {player.stats.currentLevel.level} * 1000");

        totalScore += player.stats.currentItemCnt * 100; 
        Debug.Log($"totalScore : {totalScore} += 보유 아이템 수 {player.stats.currentItemCnt} * 100");

        totalScore += (int)(Time.time - gameStartTime) * 100;
        Debug.Log($"totalScore : {totalScore} += 시간 {playTime} * 100");

        totalScore += player.stats.currentKickScore;
        Debug.Log($"totalScore : {totalScore} += 찬 적 점수 {player.stats.currentKickScore}");

        Debug.Log($"totalScore : {totalScore}");
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
