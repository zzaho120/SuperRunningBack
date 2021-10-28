using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIContorller : UIController
{
    private PlayerController player;
    private GameObject EndLine;

    private Slider[] sliders;
    private Text[] texts;
    private float totalDistance;
    private Vector3 startPosition;
    private Transform dumbbelUI;
    private float totalScoreDelayTime = 0.5f;
    private float statusWidth = 160f;
    private float statusHeight = 120f;

    private int startTotalScore;
    private PlayerStats playerStats;

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
        sliders = GetComponentsInChildren<Slider>();
        texts = GetComponentsInChildren<Text>();
        dumbbelUI = sliders[2].transform;
        playerStats = player.stats;

        totalDistance = DistanceToEndLine;
        startPosition = player.transform.position;

        SetSizeStatusBar();

        texts[5].text = $"0";
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        sliders[0].value = DistanceToStartLine / totalDistance;
        sliders[1].value = playerStats.CurrentWeightByPower;
        sliders[2].value = playerStats.CurrentItemByNextLevel;

        texts[0].text = $"{GameManager.Instance.playTime}";
        texts[2].text = $"{playerStats.currentWeight}";
        texts[3].text = $"{playerStats.currentLevel.power}";
        texts[4].text = $"{playerStats.currentLevel.level} Level";

        if (tutorialValueDirection)
            sliders[3].value -= Time.deltaTime;
        else
            sliders[3].value += Time.deltaTime;

        if (sliders[3].value >= 1.0f || sliders[3].value <= 0f)
            tutorialValueDirection = !tutorialValueDirection;
    }

    private void FixedUpdate()
    {
        dumbbelUI.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, -1f - 0.4f * player.stats.currentLevel.level, 0));
    }

    public void TutorialBarOff()
    {
        StartCoroutine(CoTutorialBarOff());
    }

    private IEnumerator CoTutorialBarOff()
    {
        var images = sliders[3].gameObject.GetComponentsInChildren<Image>();
        var alpha = images[0].color.a;
        while(alpha >= 0f)
        {
            alpha -= Time.deltaTime;
            foreach(var elem in images)
            {
                elem.color = new Color(elem.color.r, elem.color.g, elem.color.b, alpha);
            }
            yield return null;
        }

        sliders[3].gameObject.SetActive(false);
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
            texts[5].text = $"{score}";

            yield return null;
        }

        startTotalScore = totalScore;
    }

    public void SetSizeStatusBar()
    {
        var rectTr = sliders[1].GetComponent<RectTransform>();

        var width = statusWidth * player.stats.currentLevel.level;
        rectTr.sizeDelta = new Vector2(width, statusHeight);
    }
}