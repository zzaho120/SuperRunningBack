using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : UIController
{
    private Image[] images;
    private Text[] texts;
    private float showDelayTime = 1f;
    private float totalScoreDelayTime = 2f;
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

        for (int idx = 1; idx < texts.Length; idx++)
        {
            var text = texts[idx];
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }

        var min = scoreManager.finishTime / 60;
        var sec = scoreManager.finishTime % 60;

        texts[2].text = $"Play Time : {min} min {sec} sec";

        texts[3].text = $"Item : {scoreManager.itemNumber}";

        texts[4].text = $"Hold Enemy : {scoreManager.holdEnemyNumber}";

        texts[5].text = $"Kick Enemy : {scoreManager.kickEnemyNumber}";
        
        StartCoroutine(CoStartShowResult());
    }

    public override void Close()
    {
        base.Close();
    }

    private IEnumerator CoStartShowResult()
    {
        yield return new WaitForSeconds(showDelayTime);

        for (int idx = 2; idx < texts.Length; idx++)
        {
            var text = texts[idx];
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

            var image = images[idx + 1];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            yield return new WaitForSeconds(showDelayTime);
        }

        texts[1].color = new Color(texts[1].color.r, texts[1].color.g, texts[1].color.b, 1f);
        images[2].color = new Color(images[2].color.r, images[2].color.g, images[2].color.b, 1f);

        var timer = 0f;
        var totalScore = scoreManager.GetTotalScore();
        while(timer < totalScoreDelayTime)
        {
            timer += Time.deltaTime;

            var score = (int)Mathf.Lerp(0f, totalScore, timer / totalScoreDelayTime);
            texts[1].text = score.ToString();

            yield return null;
        }
    }
}
