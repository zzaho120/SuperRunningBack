using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : UIController
{
    private Image[] images;
    private Text[] texts;
    private float showDelayTime = 0.5f;
    private float totalScoreDelayTime = 0.5f;
    private ScoreManager scoreManager;

    public override void Open()
    {
        base.Open();
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
        scoreManager = GameManager.Instance.scoreManager;

        for(int idx = 3; idx < images.Length; idx++)
        {
            var image = images[idx];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }

        for (int idx = 2; idx < texts.Length; idx++)
        {
            var text = texts[idx];
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }

        var min = scoreManager.finishTime / 60;
        var sec = scoreManager.finishTime % 60;

        texts[2].text = $"Time : {min} min {sec} sec";

        texts[3].text = $"Player level : {scoreManager.playerLevel}";

        texts[4].text = $"Item : {scoreManager.itemNumber}";

        texts[5].text = $"Hold Enemy : {scoreManager.holdEnemyNumber}";

        texts[6].text = $"Kick Enemy : {scoreManager.kickEnemyNumber}";
        
        StartCoroutine(CoStartShowResult());
    }

    public override void Close()
    {
        base.Close();
    }

    private IEnumerator CoStartShowResult()
    {
        var startTotalScore = 0;
        var totalScore = 0;
            
        var scoreList = scoreManager.ScoreList;
        var scoreListIdx = 0;

        scoreManager.GetTotalScore();
        for (int idx = 2; idx < texts.Length - 1; idx++, scoreListIdx++)
        {
            yield return new WaitForSeconds(showDelayTime);

            var text = texts[idx];
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

            var image = images[idx + 1];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            yield return new WaitForSeconds(showDelayTime);

            var timer = 0f;

            if(scoreListIdx < scoreList.Count)
                totalScore += scoreList[scoreListIdx];

            while (timer < totalScoreDelayTime)
            {
                timer += Time.deltaTime;

                var score = (int)Mathf.Lerp(startTotalScore, totalScore, timer / totalScoreDelayTime);
                texts[1].text = score.ToString();

                if (scoreList[scoreListIdx] != 0)
                    yield return null;
            }

            startTotalScore = totalScore;
        }

        yield return new WaitForSeconds(showDelayTime);
        texts[texts.Length - 1].color = new Color(texts[texts.Length - 1].color.r, texts[texts.Length - 1].color.g, texts[texts.Length - 1].color.b, 1f);
        images[images.Length - 1].color = new Color(images[images.Length - 1].color.r, images[images.Length - 1].color.g, images[images.Length - 1].color.b, 1f);
    }
}
