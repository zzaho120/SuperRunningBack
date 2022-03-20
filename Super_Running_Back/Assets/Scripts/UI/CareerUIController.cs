using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CareerUIController : UIController
{
    public int totalTime;
    public int totalItem;
    public int totalHoldEnemy;
    public int totalKickEnemy;
    public int totalScore;

    public Slider currentStageBar;
    public Text currentStageText;

    private Text[] texts;
    public override void Open()
    {
        base.Open();

        totalTime = DataManager.totalTime;
        totalItem = DataManager.totalItem;
        totalHoldEnemy = DataManager.totalHoldEnemy;
        totalKickEnemy = DataManager.totalKickEnemy;
        totalScore = DataManager.totalScore;

        texts = GetComponentsInChildren<Text>();

        var hour = totalTime / 3600;
        var min = (totalTime % 3600) / 60;
        var sec = totalTime % 60;
        texts[1].text = $"Total Time : {hour}h {min}m {sec}sec";
        texts[2].text = $"Total Item : {totalItem}";
        texts[3].text = $"Hold Enemy : {totalHoldEnemy}";
        texts[4].text = $"Kick Enemy : {totalKickEnemy}";
        texts[5].text = $"Total Score : {totalScore}";
    }

    public override void Close()
    {
        base.Close();
    }
}