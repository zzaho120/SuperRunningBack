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
    public List<int> ScoreList;

    private int clearScore;
    private int holdScore = 200;
    private int KickScore = 100;
    private int itemScore = 100;
    private int levelScore = 1000;


    public void SetFinishTime(int time)
    {
        finishTime = time;
        var yardInGround = GameManager.Instance.yardInGround;
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

    public void SetHoldEnemyNumber()
    {
        holdEnemyNumber++;
    }

    public void SetKickEnemyNumber()
    {
        kickEnemyNumber++;
    }

    public void SetItemNumber(int itemNum)
    {
        if (itemNum > 0)
            itemNumber = itemNum;
    }

    public void SetPlayerLevel(int level)
    {
        if (level > 0)
            playerLevel = level;
    }
    public void GetTotalScore()
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

    private int GetClearScore()
    {
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
        return playerLevel * levelScore;
    }
}