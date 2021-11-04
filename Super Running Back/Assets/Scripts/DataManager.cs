using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DataManager
{
    private static int currentChapterIndex;
    public static int CurrentChapterIndex
    {
        get
        {
            return currentChapterIndex;
        }
        set
        {
            if (SceneManager.sceneCount > value)
            {
                currentStageIndex = value;
                CurrentStageIndex = 0;
            }
        }
    }
    private static int currentStageIndex;
    public static int CurrentStageIndex
    {
        get
        {
            return currentStageIndex;
        }

        set
        {
            currentStageIndex = value;
        }
    }

    // 커리어 UI에 사용
    public static int totalTime;
    public static int totalItem;
    public static int totalHoldEnemy;
    public static int totalKickEnemy;
    public static int totalScore;

    public static void SaveCareer(int time, int item, int hold, int kick, int score)
    {
        totalTime += time;
        totalItem += item;
        totalHoldEnemy += hold;
        totalKickEnemy += kick;
        totalScore += score;
    }
}