using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : UIController
{
    public List<AudioClip> resultSounds;
    public AudioClip scoreSound;

    public GameObject stageButton;
    public Image stageImage;
    public Text stageText;

    public GameObject chapterButton;
    public Image chapterImage;
    public Text chapterText;

    private Image[] images;
    private Text[] texts;
    private float showDelayTime = 0.5f;
    private float totalScoreDelayTime = 1f;
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

        texts[5].text = $"Hold Enemy : {scoreManager.holdEnemyWeight}";

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

        var audio = GetComponent<AudioSource>();
        audio.clip = scoreSound;

        for (int idx = 2; idx < texts.Length; idx++, scoreListIdx++)
        {
            audio.PlayOneShot(resultSounds[0]);

            var text = texts[idx];
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

            var image = images[idx + 1];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            var timer = 0f;

            if(scoreListIdx < scoreList.Count)
                totalScore += scoreList[scoreListIdx];

            audio.Play();
            while (timer < totalScoreDelayTime)
            {
                timer += Time.deltaTime;

                var score = (int)Mathf.Lerp(startTotalScore, totalScore, timer / totalScoreDelayTime);
                texts[1].text = score.ToString();

                if (scoreList[scoreListIdx] != 0)
                    yield return null;
            }
            audio.Stop();

            startTotalScore = totalScore;
        }

        foreach(var elem in resultSounds)
        {
            audio.PlayOneShot(elem);
        }

        yield return new WaitForSeconds(showDelayTime);

        if(DataManager.IsMaxStage)
        {
            stageButton.SetActive(false);
            chapterButton.SetActive(true);
        }
        else
        {
            stageButton.SetActive(true);
            chapterButton.SetActive(false);
        }
    }
}