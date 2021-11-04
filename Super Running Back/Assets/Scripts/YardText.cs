using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YardText : MonoBehaviour
{
    public TextMesh yardText;

    public void Init()
    {
        var yard = GameManager.Instance.randomGenerateStage.currentStageInfo.yard;
        yardText.text = $"{yard} yd";
    }
}
