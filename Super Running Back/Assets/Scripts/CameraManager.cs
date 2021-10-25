using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 16f, -8f);
    private float cameraCorrection = 10f;

    void FixedUpdate()
    {
        var target = GameManager.Instance.player.transform;
        transform.position = target.position + offset;
        var newTarget = new Vector3(target.position.x, 
            target.position.y, 
            target.position.z + cameraCorrection);
        transform.LookAt(newTarget);
    }
}
