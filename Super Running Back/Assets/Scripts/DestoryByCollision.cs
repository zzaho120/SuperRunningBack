using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByCollision : MonoBehaviour, ICollisable
{
    public float destroyDelayTime;
    public void onActionByCollision(GameObject other)
    {
        Destroy(gameObject, destroyDelayTime);
    }
}
