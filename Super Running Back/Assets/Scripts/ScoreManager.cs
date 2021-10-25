using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int finishTime;
    private int holdEnemyNumber;
    private int kickEnemyNumber;
    private int itemNumber;
    private bool isClear;

    private int clearScore = 5000;
    private int holdScore = 200;
    private int KickScore = 100;
    private int itemScore = 100;

    public void SetFinishTime(int time)
    {
        finishTime = time;
    }

    public void SetHoldEnemyNumber(int weight)
    {
        if (weight > 0)
            holdEnemyNumber += weight;
    }

    public void SetKickEnemyNumber(int enemyNumber)
    {
        if (enemyNumber > 0)
            kickEnemyNumber += enemyNumber;
    }

    public void SetItemNumber(int itemNum)
    {
        if (itemNum > 0)
            itemNumber = itemNum;
    }

    public int GetTotalScore()
    {
        var result = 0;

        if (isClear)
            result += clearScore;

        result += holdEnemyNumber * holdScore;
        result += kickEnemyNumber * KickScore;
        result += itemNumber * itemScore;

        return result;
    }
}