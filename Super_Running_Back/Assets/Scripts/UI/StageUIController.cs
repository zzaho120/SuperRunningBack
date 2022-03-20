using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : UIController
{
    public Slider currentStageBar;
    public Text currentStageText;
    public List<Image> stageImage;
    public List<Image> trophyImage;
    public List<Sprite> sprites;
    public SimpleScrollSnap snap;
    public override void Open()
    {
        base.Open();
        snap.TargetPanel = DataManager.CurrentChapterIdx;
        currentStageBar.value = DataManager.CurrentStageIdx / (float)(DataManager.maxStageIdx - 1);
        currentStageText.text = ((SceneName)snap.TargetPanel).ToString() + " League";


        for(int idx = 0; idx < trophyImage.Count; idx++)
        {
            var color = trophyImage[idx].color;
            trophyImage[idx].color = new Color(color.r, color.g, color.b, 0.4f);
        }

        for (int idx = 0; idx < DataManager.CurrentChapterIdx; idx++)
        {
            var color = trophyImage[idx].color;
            trophyImage[idx].color = new Color(color.r, color.g, color.b, 1f);
        }

        for (int idx = 0; idx < stageImage.Count; idx++)
        {
            stageImage[idx].sprite = sprites[0];
        }

        for(int idx = 0; idx < DataManager.CurrentStageIdx; idx++)
        {
            stageImage[idx].sprite = sprites[1];
        }
    }

    public void OnChangeUI()
    {
        if(DataManager.CurrentChapterIdx > snap.TargetPanel)
        {
            currentStageBar.value = 1f;

            for (int idx = 0; idx < stageImage.Count; idx++)
            {
                stageImage[idx].sprite = sprites[1];
            }
        }
        else if(DataManager.CurrentChapterIdx == snap.TargetPanel)
        {
            currentStageBar.value = DataManager.CurrentStageIdx / (float)(DataManager.maxStageIdx - 1);

            for (int idx = 0; idx < stageImage.Count; idx++)
            {
                stageImage[idx].sprite = sprites[0];
            }

            for (int idx = 0; idx < DataManager.CurrentStageIdx; idx++)
            {
                stageImage[idx].sprite = sprites[1];
            }
        }
        else
        {
            currentStageBar.value = 0f;

            for (int idx = 0; idx < stageImage.Count; idx++)
            {
                stageImage[idx].sprite = sprites[0];
            }
        }

        currentStageText.text = ((SceneName)snap.TargetPanel).ToString() + " League";
    }
}