using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YardText : MonoBehaviour
{
    public TextMesh yardText;

    private void Start()
    {
        var yard = GameManager.Instance.randomGenerateStage.stageInfo.yard;
        yardText.text = $"{yard} yd";
    }
}
