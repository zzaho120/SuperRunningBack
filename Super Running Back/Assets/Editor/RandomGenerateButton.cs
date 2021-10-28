using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomGenerateStage))]
public class RandomGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RandomGenerateStage generator = (RandomGenerateStage)target;
        if (GUILayout.Button("Generate Random"))
        {
            generator.Generate();
        }
    }

}
