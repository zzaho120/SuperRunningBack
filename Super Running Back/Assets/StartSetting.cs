using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{
    public List<Transform> transforms;
    private Vector3 correctionPos;
    public void GameStartInit(float yard)
    {
        yard = (yard * 0.1f) - 5f;

        correctionPos = new Vector3(0f, 0f, yard * 35);

        foreach(var transform in transforms)
        {
            transform.position -= correctionPos;
        }
    }

    public void PlayerResultSetting()
    {
        foreach(var transform in transforms)
        {
            if(transform.gameObject.tag == "Player")
            {
                transform.position += correctionPos;
            }
        }
    }
}
