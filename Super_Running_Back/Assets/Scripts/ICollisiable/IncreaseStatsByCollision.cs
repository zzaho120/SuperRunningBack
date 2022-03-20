using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatsByCollision : MonoBehaviour, ICollisable
{
    public void OnActionByCollision(GameObject other)
    {
        GameManager.Instance.MsgGetItem();
    }
}