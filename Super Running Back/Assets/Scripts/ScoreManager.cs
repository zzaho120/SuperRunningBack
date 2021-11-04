using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int finishTime;
    public int itemNumber;
    public int holdEnemyNumber;
    public int kickEnemyNumber;
    public int playerLevel;
    public int totalScore;
    public List<int> ScoreList;

    private int clearScore;
    private int holdScore = 200;
    private int KickScore = 100;
    private int itemScore = 100;
    private int levelScore = 1000;

    public void Init()
    {
        finishTime = 0;
        itemNumber = 0;
        holdEnemyNumber = 0;
        kickEnemyNumber = 0;
        playerLevel = 0;
        totalScore = 0;
    }
    public void SetFinishTime(int time)
    {
        finishTime = time;
        var yardInGround = GameManager.Instance.randomGenerateStage.currentStageInfo.yard;
        switch (yardInGround)
        {
            case 50:
                if (finishTime <= 15)
                    clearScore = 15000;
                else if (finishTime <= 25)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
            case 60:
                if (finishTime <= 20)
                    clearScore = 15000;
                else if (finishTime <= 30)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
            case 70:
                if (finishTime <= 25)
                    clearScore = 15000;
                else if (finishTime <= 35)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
            case 80:
                if (finishTime <= 30)
                    clearScore = 15000;
                else if (finishTime <= 45)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
            case 90:
                if (finishTime <= 45)
                    clearScore = 15000;
                else if (finishTime <= 55)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
            case 100:
                if (finishTime <= 50)
                    clearScore = 15000;
                else if (finishTime <= 60)
                    clearScore = 10000;
                else
                    clearScore = 5000;
                break;
        }
    }

    public void AddHoldEnemyNumber()
    {
        holdEnemyNumber++;
        totalScore += holdScore;
    }

    public void AddKickEnemyNumber()
    {
        kickEnemyNumber++;
        totalScore += KickScore;
    }

    public void AddItemNumber()
    {
        itemNumber++;
        totalScore += itemScore;
    }

    public void SetPlayerLevel(int level)
    {
        if (level > 0)
            playerLevel = level;
    }
    public void SetTotalScore()
    {
        if (ScoreList == null)
            ScoreList = new List<int>();
        else
            ScoreList.Clear();

        ScoreList.Add(GetClearScore());
        ScoreList.Add(GetLevelScore());
        ScoreList.Add(GetItemScore());
        ScoreList.Add(GetHoldEnemyScore());
        ScoreList.Add(GetKickEnemyScore());
    }
    
    public int GetTotalScore()
    {
        return totalScore;
    }

    private int GetClearScore()
    {
        totalScore += clearScore;
        return clearScore;
    }

    private int GetHoldEnemyScore()
    {
        return holdEnemyNumber * holdScore;
    }

    private int GetKickEnemyScore()
    {
        return kickEnemyNumber * KickScore;
    }

    private int GetItemScore()
    {
        return itemNumber * itemScore;
    }
    
    private int GetLevelScore()
    {
        var levelScore = playerLevel * this.levelScore;
        totalScore += levelScore;
        return levelScore;
    }
}