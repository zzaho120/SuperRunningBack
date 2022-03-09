using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIContorller : UIController
{
    private PlayerController player;
    private GameObject EndLine;

    public Slider destinationBar;
    public Slider tutorialBar;
    public Slider playerStatusBar;
    public TextMeshProUGUI stageNameText;
    public TextMeshProUGUI playerStatusText;
    private float totalDistance;
    private Vector3 startPosition;
    private bool tutorialValueDirection;
    public RectTransform statusObjTr;

    private enum PlayerPower
    {
        Weak = 2,
        Normal = 6,
        Strong = 9,
        VeryStrong = 13,
        Powerfull = 18
    }


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
        stageNameText.text = $"STAGE {DataManager.StageIdx}";
        statusObjTr.anchoredPosition = new Vector2(0f, 100f);

        ChangePlayerStatusBar();
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        destinationBar.value = DistanceToStartLine / totalDistance;

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

    public void MsgGetItem()
    {
        var rectTr = playerStatusBar.GetComponent<RectTransform>();
        rectTr.anchoredPosition += new Vector2(0f, 40f);

        ChangePlayerStatusBar();
    }

    public void MsgGetPenalty()
    {
        var rectTr = playerStatusBar.GetComponent<RectTransform>();
        rectTr.anchoredPosition += new Vector2(0f, -15f);

        ChangePlayerStatusBar();
    }

    public void ChangePlayerStatusBar()
    {
        var playerStats = GameManager.Instance.player.stats;
        var max = playerStats.maxSpeed - playerStats.gameoverSpeed;
        var curSpeed = playerStats.currentSpeed - playerStats.gameoverSpeed;
        var t = curSpeed / max;
        playerStatusBar.value = t;

        ChangePlayerStatusText((int)curSpeed);
    }

    public void ChangePlayerStatusText(int curSpeed)
    {
        if (curSpeed <= (int)PlayerPower.Weak)
            playerStatusText.text = "Weak";
        else if (curSpeed <= (int)PlayerPower.Normal)
            playerStatusText.text = "Normal";
        else if (curSpeed <= (int)PlayerPower.Strong)
            playerStatusText.text = "Strong";
        else if (curSpeed <= (int)PlayerPower.VeryStrong)
            playerStatusText.text = "Very Strong";
        else if (curSpeed <= (int)PlayerPower.Powerfull)
            playerStatusText.text = "Powerfull";
    }
}