using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByCollision : MonoBehaviour, ICollisable
{
    public float destroyDelayTime;
    public PoolName poolName;
    private bool isDestroy;

    public void Init()
    {
        isDestroy = false;
    }
    public void onActionByCollision(GameObject other)
    {
        if(!isDestroy)
        {
            isDestroy = true;
            GameManager.Instance.ReturnListObject(gameObject);
            ObjectPool.ReturnObject(poolName, gameObject);
        }
    }
}
