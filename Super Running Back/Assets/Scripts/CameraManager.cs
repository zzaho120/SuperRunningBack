using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3 offset;
    public float cameraCorrection;
    private Vector3 levelCorrection;
    private Transform startCamera;
    private int playerLevel;

    private float rotateValue;
    private float rotateInput;

    private void Awake()
    {
        startCamera = GameObject.FindGameObjectWithTag("StartCamera").transform;
        levelCorrection = offset * 0.1f;
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
        var newTarget = new Vector3(target.position.x, 
            target.position.y, 
            target.position.z + cameraCorrection);
        transform.LookAt(newTarget);
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
