using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerateStage : MonoBehaviour
{
    public AreaManager[] areas;



    public void Generate() 
    {
        foreach(var elem in areas)
        {
            elem.Awake();
            elem.Generate();
        }
    }
}