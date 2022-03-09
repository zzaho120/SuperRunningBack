using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{
    private Transform cameraTr;
    private Vector3 originPos;
    private float duration = 1.5f;
    private IEnumerator curCoroutine;
    private TextMesh textMesh;
    private Color color;
    private Vector3 levelCorrection = new Vector3(0f, 0.7f, 0f);
    private void Start()
    {
        cameraTr = GameManager.Instance.mainCamera.transform;

        textMesh = GetComponent<TextMesh>();
        color = textMesh.color;
        textMesh.color = new Color(color.r, color.g, color.b, 0f);

        originPos = transform.localPosition;
    }

    public void ShowLevelUp()
    {
        var level = GameManager.Instance.player.stats.currentLevel.level;
        transform.localPosition = originPos + levelCorrection * (level - 1);
        textMesh.color = new Color(color.r, color.g, color.b, 1f);
        if (curCoroutine != null)
        {
            StopCoroutine(CoShowText());
            curCoroutine = null;
        }

        StartCoroutine(CoShowText());
    }

    private IEnumerator CoShowText()
    {
        var startTime = Time.time;
        while(startTime + duration > Time.time)
        {
            transform.position += Vector3.up * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(cameraTr.forward);
            yield return null;
        }
        textMesh.color = new Color(color.r, color.g, color.b, 0f);
    }
}
