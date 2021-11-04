using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{
    public GameObject[] transforms;
    public Transform[] originTr;
    private Vector3 correctionPos;
    private float stageYard;
    public void GameStartInit(float yard)
    {
        stageYard = yard;
        stageYard = (stageYard * 0.1f) - 5f;
    }

    public void Init()
    {
        correctionPos = new Vector3(0f, 0f, stageYard * 35);

        for(int idx = 0; idx < originTr.Length; idx++)
        {
            transforms[idx].transform.position = originTr[idx].position;
            transforms[idx].transform.rotation = originTr[idx].rotation;
        }

        foreach (var transform in transforms)
        {
            Debug.Log(transform.gameObject);
            transform.transform.position -= correctionPos;
        }
    }
}
