using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByCollision : MonoBehaviour, ICollisable
{
    public float destroyDelayTime;
    public PoolName poolName;
    public void onActionByCollision(GameObject other)
    {
        ObjectPool.ReturnObject(poolName, gameObject);
    }
}
