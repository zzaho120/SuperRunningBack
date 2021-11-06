using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static void Load(int scene)
    {
        if(DataManager.maxChaperIdx > scene)
            SceneManager.LoadScene(scene);
        else
        {
            SceneManager.LoadScene(DataManager.CurrentChapterIdx);
        }
    }
}