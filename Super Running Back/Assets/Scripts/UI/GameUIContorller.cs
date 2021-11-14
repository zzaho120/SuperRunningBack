using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIContorller : UIController
{
    private PlayerController player;
    private GameObject EndLine;

    public Slider destinationBar;
    public Slider tutorialBar;
    public Text timeText;
    public Text scoreText;
    private float totalDistance;
    private Vector3 startPosition;
    private float totalScoreDelayTime = 0.5f;

    private int startTotalScore;

    private bool tutorialValueDirection;

    public float DistanceToEndLine
    {
        get
        {
            if (player != null && EndLine != null)
                return Vector3.Distance(player.transform.position, EndLine.transform.position);
            return 0;
        }
    }

    public float DistanceToStartLine
    {
        get
        {
            if (player != null)
                return Vector3.Distance(startPosition, player.transform.position);
            return 0;
        }
    }

    public override void Open()
    {
        base.Open();
        player = GameManager.Instance.player;
        EndLine = GameObject.FindWithTag("Finish");

        totalDistance = DistanceToEndLine;
        startPosition = player.transform.position;

        scoreText.text = $"0";
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        destinationBar.value = DistanceToStartLine / totalDistance;

        timeText.text = $"{GameManager.Instance.playTime}";

        if (tutorialValueDirection)
            tutorialBar.value -= Time.deltaTime;
        else
            tutorialBar.value += Time.deltaTime;

        if (tutorialBar.value >= 1.0f || tutorialBar.value <= 0f)
            tutorialValueDirection = !tutorialValueDirection;
    }

    public void TutorialBarOff()
    {
        StartCoroutine(CoTutorialBarOff());
    }

    private IEnumerator CoTutorialBarOff()
    {
        var images = tutorialBar.gameObject.GetComponentsInChildren<Image>();
        var alpha = images[0].color.a;
        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime;
            foreach (var elem in images)
            {
                elem.color = new Color(elem.color.r, elem.color.g, elem.color.b, alpha);
            }
            yield return null;
        }
    }

    public void IncreaseScore()
    {
        StartCoroutine(CoPrintScore());
    }

    public void StopIncreasceScore()
    {
        StopAllCoroutines();
    }

    private IEnumerator CoPrintScore()
    {
        var timer = 0f;
        var totalScore = GameManager.Instance.scoreManager.totalScore;
        while (timer < totalScoreDelayTime)
        {
            timer += Time.deltaTime;

            var score = (int)Mathf.Lerp(startTotalScore, totalScore, timer / totalScoreDelayTime);
            scoreText.text = $"{score}";

            yield return null;
        }

        startTotalScore = totalScore;
    }
}