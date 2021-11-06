using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverUIController : UIController
{
    private Image[] images;
    private Text[] texts;
    private bool isAlphaValue;
    public override void Open()
    {
        base.Open();
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        foreach (var elem in images)
        {
            elem.color = new Color(elem.color.r, elem.color.g, elem.color.b, 0f);
        }
        foreach(var elem in texts)
        {
            elem.color = new Color(elem.color.r, elem.color.g, elem.color.b, 0f);
        }
        

        StartCoroutine(CoAlphaUI());
    }

    public override void Close()
    {
        base.Close();
    }

    private IEnumerator CoAlphaUI()
    {
        var alpha = 0f;
        while(alpha < 1f)
        {
            alpha = alpha + Time.deltaTime;

            if(alpha < 0.5f)
                images[0].color = new Color(images[0].color.r, images[0].color.g, images[0].color.b, alpha);

            for(int idx = 1; idx < images.Length; idx++)
            {
                var image = images[idx];
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }

            foreach (var elem in texts)
            {
                elem.color = new Color(elem.color.r, elem.color.g, elem.color.b, alpha);
            }

            yield return null;
        }
    }
}
