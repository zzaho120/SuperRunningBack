using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : UIController
{
    public Slider currentStageBar;
    public Text currentStageText;
    public override void Open()
    {
        base.Open();
        currentStageBar.value = DataManager.CurrentStageIdx / (float)(DataManager.maxStageIdx - 1);
        currentStageText.text = ((SceneName)DataManager.CurrentChapterIdx).ToString() + " Leauge";
    }
}