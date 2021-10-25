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

        totalDistance = DistanceToEndLine;
        startPosition = player.transform.position;
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        sliders[0].value = DistanceToStartLine / totalDistance;
        sliders[1].value = player.stats.CurrentWeightByPower;
        sliders[2].value = player.stats.CurrentItemByNextLevel;
        texts[0].text = GameManager.Instance.playTime.ToString();
        texts[2].text = player.stats.currentWeight.ToString();
        texts[3].text = player.stats.currentLevel.power.ToString();
        texts[4].text = $"{player.stats.currentLevel.level} Level";

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
}