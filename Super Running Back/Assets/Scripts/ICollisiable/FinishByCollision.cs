using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishByCollision : MonoBehaviour, ICollisable
{
    public UnityEvent onFinishEvent;
    public void onActionByCollision(GameObject other)
    {
        onFinishEvent.Invoke();
    }
}