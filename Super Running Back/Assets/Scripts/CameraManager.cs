using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 16f, -8f);
    private Vector3 levelCorrection = new Vector3(0f, 1f, -0.5f);
    private float cameraCorrection = 10f;
    private float maxRotateValue = 5f;
    private Transform startCamera;
    private int playerLevel;

    private float rotateValue;
    private float rotateInput;

    private void Awake()
    {
        startCamera = GameObject.FindGameObjectWithTag("StartCamera").transform;
        transform.position = startCamera.position;
        transform.rotation = startCamera.rotation;
    }

    private void Start()
    {
        InitPlayerLevel();
    }

    void FixedUpdate()
    {
        var target = GameManager.Instance.player.transform;
        transform.position = target.position + offset + (levelCorrection * playerLevel);
        var newTarget = new Vector3(target.position.x + rotateValue, 
            target.position.y, 
            target.position.z + cameraCorrection);
        transform.LookAt(newTarget);
    }

    public void RotateCameraView(float horizontal)
    {
        horizontal += 1.0f;
        horizontal *= 0.5f;
        rotateValue = Mathf.Lerp(-maxRotateValue, maxRotateValue, horizontal);
    }

    private void InitPlayerLevel()
    {
        var initLevel = GameManager.Instance.player.stats.initLevel;
        if (initLevel > 0)
            playerLevel = initLevel - 1;
        else
            playerLevel = 0;
    }

    public void SetPlayerLevel(int level)
    {
        if(level > 0)
            playerLevel = level - 1;
    }

    
}
