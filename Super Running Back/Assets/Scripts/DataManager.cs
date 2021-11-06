using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int stageIdx;
    public int chapterIdx;

    public int totalTime;
    public int totalItem;
    public int totalHoldEnemy;
    public int totalKickEnemy;
    public int totalScore;
}

public static class DataManager
{
    private static bool isLoadData = false;
    private static bool isLoadScene = false;
    public static int maxChaperIdx = 5;
    public static int maxStageIdx = 6;
    private static int currentChapterIdx;
    public static int CurrentChapterIdx
    {
        get
        {
            return currentChapterIdx;
        }
        set
        {
            if (maxChaperIdx > value)
            {
                currentChapterIdx = value;
                CurrentStageIdx = 0;
            }
            else
            {
                Init();
            }
        }
    }
    private static int currentStageIdx;
    public static int CurrentStageIdx
    {
        get
        {
            return currentStageIdx;
        }

        set
        {
            if (maxStageIdx > value)
            {
                currentStageIdx = value;
            }
            else
            {
                CurrentChapterIdx++;
            }
        }
    }

    public static bool IsMaxStage
    {
        get
        {
            return CurrentStageIdx == maxStageIdx - 1;
        }
    }

    // 커리어 UI에 사용
    public static int totalTime;
    public static int totalItem;
    public static int totalHoldEnemy;
    public static int totalKickEnemy;
    public static int totalScore;

    public static void Init()
    {
        CurrentChapterIdx = 0;
        CurrentStageIdx = 0;
    }

    public static void NextStage()
    {
        CurrentStageIdx++;
        DataManager.SaveData();
    }

    public static void SaveCareer(int time, int item, int hold, int kick, int score)
    {
        totalTime += time;
        totalItem += item;
        totalHoldEnemy += hold;
        totalKickEnemy += kick;
        totalScore += score;
    }

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Save.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData();

        data.chapterIdx = CurrentChapterIdx;
        data.stageIdx = CurrentStageIdx;

        data.totalTime = totalTime;
        data.totalItem = totalItem;
        data.totalHoldEnemy = totalHoldEnemy;
        data.totalKickEnemy = totalKickEnemy;
        data.totalScore = totalScore;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadData()
    {
        if(!isLoadData)
        {
            string path = Application.persistentDataPath + "/Save.data";
            if (File.Exists(path))
            {
                isLoadData = true;
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                GameData data = formatter.Deserialize(stream) as GameData;
                stream.Close();

                CurrentChapterIdx = data.chapterIdx;
                CurrentStageIdx = data.stageIdx;

                totalTime = data.totalTime;
                totalItem = data.totalItem;
                totalHoldEnemy = data.totalHoldEnemy;
                totalKickEnemy = data.totalKickEnemy;
                totalScore = data.totalScore;
            }
            else
            {
                Debug.LogError("Save File not found in " + path);
            }
        }
    }

    public static void LoadScene()
    {
        if(!isLoadScene && CurrentChapterIdx != (int)SceneName.Elemantary)
        {
            isLoadScene = true;
            Loader.Load(CurrentChapterIdx);
        }
    }
}